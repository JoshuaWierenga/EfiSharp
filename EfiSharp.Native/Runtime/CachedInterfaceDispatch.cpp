// Licensed to the.NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// ==--==
//
// Shared (non-architecture specific) portions of a mechanism to perform interface dispatch using an alternate
// mechanism to VSD that does not require runtime generation of code.
//
// ============================================================================
// Changes made by Joshua Wierenga.

// We always allocate cache sizes with a power of 2 number of entries. We have a maximum size we support,
// defined below.
#define CID_MAX_CACHE_SIZE_LOG2 6
#define CID_MAX_CACHE_SIZE      (1 << CID_MAX_CACHE_SIZE_LOG2)

#define CID_COUNTER_INC(_counter_name)

#ifdef FEATURE_CACHED_INTERFACE_DISPATCH

#include "CommonMacros.h"
//TODO remove inc/ to match runtime version
#include "inc/daccess.h"
#include "inc/rhbinder.h"

#include "CachedInterfaceDispatch.h"

//TODO Add PalRedhawk.h
#include "uefi/PalRedhawkInline.h"

// Helper function for updating two adjacent pointers (which are aligned on a double pointer-sized boundary)
// atomically.
//
// This is used to update interface dispatch cache entries and also the stub/cache pair in
// interface dispatch indirection cells. The cases have slightly different semantics: cache entry updates
// (fFailOnNonNull == true) require that the existing values in the location are both NULL whereas indirection
// cell updates have no such restriction. In both cases we'll try the update once; on failure we'll return the
// new value of the second pointer and on success we'll the old value of the second pointer.
//
// This suits the semantics of both callers. For indirection cell updates the caller needs to know the address
// of the cache that can now be scheduled for release and the cache pointer is the second one in the pair. For
// cache entry updates the caller only needs a success/failure indication: on success the return value will be
// NULL and on failure non-NULL.
static void* UpdatePointerPairAtomically(void* pPairLocation,
	void* pFirstPointer,
	void* pSecondPointer,
	bool fFailOnNonNull)
{
#if defined(HOST_64BIT)
	// The same comments apply to the AMD64 version. The CompareExchange looks a little different since the
	// API was refactored in terms of int64_t to avoid creating a 128-bit integer type.
	int64_t rgComparand[2] = { 0 , 0 };
	if (!fFailOnNonNull)
	{
		rgComparand[0] = *(int64_t volatile*)pPairLocation;
		rgComparand[1] = *((int64_t volatile*)pPairLocation + 1);
	}

	uint8_t bResult = PalInterlockedCompareExchange128((int64_t*)pPairLocation, (int64_t)pSecondPointer, (int64_t)pFirstPointer, rgComparand);
	if (bResult == 1)
	{
		// Success, return old value of second pointer (rgComparand is updated by
		// PalInterlockedCompareExchange128 with the old pointer values in this case).
		return (void*)rgComparand[1];
	}

	// Failure, return the new second pointer value.
	return pSecondPointer;

#else
	// Stuff the two pointers into a 64-bit value as the proposed new value for the CompareExchange64 below.
	int64_t iNewValue = (int64_t)((uint64_t)(uintptr_t)pFirstPointer | ((uint64_t)(uintptr_t)pSecondPointer << 32));

	// Read the old value in the location. If fFailOnNonNull is set we just assume this was zero and we'll
	// fail below if that's not the case.
	int64_t iOldValue = fFailOnNonNull ? 0 : *(int64_t volatile*)pPairLocation;

	int64_t iUpdatedOldValue = PalInterlockedCompareExchange64((int64_t*)pPairLocation, iNewValue, iOldValue);

	if (iUpdatedOldValue == iOldValue)
	{
		// Successful update. Return the previous value of the second pointer. For cache entry updates
		// (fFailOnNonNull == true) this is guaranteed to be NULL in this case and the result being being
		// NULL in the success case is all the caller cares about. For indirection cell updates the second
		// pointer represents the old cache and the caller needs this data so they can schedule the cache
		// for deletion once it becomes safe to do so.
		return (void*)(uint32_t)(iOldValue >> 32);
	}

	// The update failed due to a racing update to the same location. Return the new value of the second
	// pointer (either a new cache that lost the race or a non-NULL pointer in the cache entry update case).
	return pSecondPointer;
#endif // HOST_64BIT
}

// Helper method for updating an interface dispatch cache entry atomically. See comments by the usage of
// this method for the details of why we need this. If a racing update is detected false is returned and the
// update abandoned. This is necessary since it's not safe to update a valid cache entry (one with a non-NULL
// m_pInstanceType field) outside of a GC.
static bool UpdateCacheEntryAtomically(InterfaceDispatchCacheEntry* pEntry,
	EEType* pInstanceType,
	void* pTargetCode)
{
	C_ASSERT(sizeof(InterfaceDispatchCacheEntry) == (sizeof(void*) * 2));
	C_ASSERT(offsetof(InterfaceDispatchCacheEntry, m_pInstanceType) < offsetof(InterfaceDispatchCacheEntry, m_pTargetCode));

	return UpdatePointerPairAtomically(pEntry, pInstanceType, pTargetCode, true) == NULL;
}


COOP_PINVOKE_HELPER(PTR_Code, RhpUpdateDispatchCellCache, (InterfaceDispatchCell* pCell, PTR_Code pTargetCode, EEType* pInstanceType, DispatchCellInfo* pNewCellInfo))
{
	// Attempt to update the cache with this new mapping (if we have any cache at all, the initial state
	// is none).
	InterfaceDispatchCache* pCache = (InterfaceDispatchCache*)pCell->GetCache();
	uint32_t cOldCacheEntries = 0;
	/*if (pCache != NULL)
	{
		InterfaceDispatchCacheEntry* pCacheEntry = pCache->m_rgEntries;
		for (uint32_t i = 0; i < pCache->m_cEntries; i++, pCacheEntry++)
		{
			if (pCacheEntry->m_pInstanceType == NULL)
			{
				if (UpdateCacheEntryAtomically(pCacheEntry, pInstanceType, pTargetCode))
					return (PTR_Code)pTargetCode;
			}
		}

		cOldCacheEntries = pCache->m_cEntries;
	}

	// Failed to update an existing cache, we need to allocate a new cache. The old one, if any, might
	// still be in use so we can't simply reclaim it. Instead we keep it around until the next GC at which
	// point we know no code is holding a reference to it. Particular cache sizes are associated with a
	// (globally shared) stub which implicitly knows the size of the cache.

	if (cOldCacheEntries == CID_MAX_CACHE_SIZE)
	{
		// We already reached the maximum cache size we wish to allocate.For now don't attempt to cache
		// the mapping we just did: there's no safe way to update the existing cache right now if it
		// doesn't have an empty entries. There are schemes that would let us do this at the next GC point
		// but it's not clear whether we should do this or re-tune the cache max size, we need to measure
		// this.
		CID_COUNTER_INC(CacheSizeOverflows);
		return (PTR_Code)pTargetCode;
	}

	uint32_t cNewCacheEntries = cOldCacheEntries ? cOldCacheEntries * 2 : 1;
	void* pStub;
	uintptr_t newCacheValue = AllocateCache(cNewCacheEntries, pCache, pNewCellInfo, &pStub);
	if (newCacheValue == 0)
	{
		CID_COUNTER_INC(CacheOutOfMemory);
		return (PTR_Code)pTargetCode;
	}

	if (InterfaceDispatchCell::IsCache(newCacheValue))
	{
		pCache = (InterfaceDispatchCache*)newCacheValue;
#if !defined(HOST_AMD64) && !defined(HOST_ARM64)
		// Set back pointer to interface dispatch cell for non-AMD64 and non-ARM64
		// for AMD64 and ARM64, we have enough registers to make this trick unnecessary
		pCache->m_pCell = pCell;
#endif // !defined(HOST_AMD64) && !defined(HOST_ARM64)

		// Add entry to the first unused slot.
		InterfaceDispatchCacheEntry* pCacheEntry = &pCache->m_rgEntries[cOldCacheEntries];
		pCacheEntry->m_pInstanceType = pInstanceType;
		pCacheEntry->m_pTargetCode = pTargetCode;
	}

	// Publish the new cache by atomically updating both the cache and stub pointers in the indirection
	// cell. This returns us a cache to discard which may be NULL (no previous cache), the previous cache
	// value or the cache we just allocated (another thread performed an update first).
	InterfaceDispatchCache* pDiscardedCache = UpdateCellStubAndCache(pCell, pStub, newCacheValue);
	if (pDiscardedCache)
		DiscardCache(pDiscardedCache);*/
	
	return (PTR_Code)pTargetCode;
}

// Given a dispatch cell, get the type and slot associated with it. This function MUST be implemented
// in cooperative native code, as the m_pCache field on the cell is unsafe to access from managed
// code due to its use of the GC state as a lock, and as lifetime control
COOP_PINVOKE_HELPER(void, RhpGetDispatchCellInfo, (InterfaceDispatchCell* pCell, DispatchCellInfo* pDispatchCellInfo))
{
	*pDispatchCellInfo = pCell->GetDispatchCellInfo();
}

#endif // FEATURE_CACHED_INTERFACE_DISPATCH