// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

//
// This header contains binder-generated data structures that the runtime consumes.
//
#include "TargetPtrs.h"

#include "../rhassert.h"
#include "../../../EfiSharp.libc/internal/include/stdint.h"

class EEType;

#ifdef FEATURE_CACHED_INTERFACE_DISPATCH

enum class DispatchCellType
{
    InterfaceAndSlot = 0x0,
    MetadataToken = 0x1,
    VTableOffset = 0x2,
};

struct DispatchCellInfo
{
    DispatchCellType CellType;
    EEType* InterfaceType = nullptr;
    uint16_t InterfaceSlot = 0;
    uint8_t HasCache = 0;
    uint32_t MetadataToken = 0;
    uint32_t VTableOffset = 0;
};

struct InterfaceDispatchCacheHeader
{
private:
    enum Flags
    {
        CH_TypeAndSlotIndex = 0x0,
        CH_MetadataToken = 0x1,
        CH_Mask = 0x3,
        CH_Shift = 0x2,
    };

public:
    void Initialize(EEType* pInterfaceType, uint16_t interfaceSlot, uint32_t metadataToken)
    {
        if (pInterfaceType != nullptr)
        {
            ASSERT(metadataToken == 0);
            m_pInterfaceType = pInterfaceType;
            m_slotIndexOrMetadataTokenEncoded = CH_TypeAndSlotIndex | (((uint32_t)interfaceSlot) << CH_Shift);
        }
        else
        {
            ASSERT(pInterfaceType == nullptr);
            ASSERT(interfaceSlot == 0);
            m_pInterfaceType = nullptr;
            m_slotIndexOrMetadataTokenEncoded = CH_MetadataToken | (metadataToken << CH_Shift);
        }
    }

    void Initialize(const DispatchCellInfo* pCellInfo)
    {
        ASSERT((pCellInfo->CellType == DispatchCellType::InterfaceAndSlot) ||
            (pCellInfo->CellType == DispatchCellType::MetadataToken));
        if (pCellInfo->CellType == DispatchCellType::InterfaceAndSlot)
        {
            ASSERT(pCellInfo->MetadataToken == 0);
            Initialize(pCellInfo->InterfaceType, pCellInfo->InterfaceSlot, 0);
        }
        else
        {
            ASSERT(pCellInfo->CellType == DispatchCellType::MetadataToken);
            ASSERT(pCellInfo->InterfaceType == nullptr);
            Initialize(nullptr, 0, pCellInfo->MetadataToken);
        }
    }

    DispatchCellInfo GetDispatchCellInfo()
    {
        DispatchCellInfo cellInfo;

        if ((m_slotIndexOrMetadataTokenEncoded & CH_Mask) == CH_TypeAndSlotIndex)
        {
            cellInfo.InterfaceType = m_pInterfaceType;
            cellInfo.InterfaceSlot = (uint16_t)(m_slotIndexOrMetadataTokenEncoded >> CH_Shift);
            cellInfo.CellType = DispatchCellType::InterfaceAndSlot;
        }
        else
        {
            cellInfo.MetadataToken = m_slotIndexOrMetadataTokenEncoded >> CH_Shift;
            cellInfo.CellType = DispatchCellType::MetadataToken;
        }
        cellInfo.HasCache = 1;
        return cellInfo;
    }

private:
    EEType* m_pInterfaceType;   // EEType of interface to dispatch on
    uint32_t      m_slotIndexOrMetadataTokenEncoded;
};

// One of these is allocated per interface call site. It holds the stub to call, data to pass to that stub
// (cache information) and the interface contract, i.e. the interface type and slot being called.
struct InterfaceDispatchCell
{
    // The first two fields must remain together and at the beginning of the structure. This is due to the
    // synchronization requirements of the code that updates these at runtime and the instructions generated
    // by the binder for interface call sites.
    UIntTarget      m_pStub;    // Call this code to execute the interface dispatch
    volatile UIntTarget m_pCache;   // Context used by the stub above (one or both of the low two bits are set
                                    // for initial dispatch, and if not set, using this as a cache pointer or
                                    // as a vtable offset.)
                                    //
                                    // In addition, there is a Slot/Flag use of this field. DispatchCells are
                                    // emitted as a group, and the final one in the group (identified by m_pStub
                                    // having the null value) will have a Slot field is the low 16 bits of the
                                    // m_pCache field, and in the second lowest 16 bits, a Flags field. For the interface
                                    // case Flags shall be 0, and for the metadata token case, Flags shall be 1.

	//
    // Keep these in sync with the managed copy in src\Common\src\Internal\Runtime\InterfaceCachePointerType.cs
    //
    enum Flags
    {
        // The low 2 bits of the m_pCache pointer are treated specially so that we can avoid the need for
        // extra fields on this type.
        // OR if the m_pCache value is less than 0x1000 then this it is a vtable offset and should be used as such
        IDC_CachePointerIsInterfaceRelativePointer = 0x3,
        IDC_CachePointerIsIndirectedInterfaceRelativePointer = 0x2,
        IDC_CachePointerIsInterfacePointerOrMetadataToken = 0x1, // Metadata token is a 30 bit number in this case.
                                                                 // Tokens are required to have at least one of their upper 20 bits set
                                                                 // But they are not required by this part of the system to follow any specific
                                                                 // token format
        IDC_CachePointerPointsAtCache = 0x0,
        IDC_CachePointerMask = 0x3,
        IDC_CachePointerMaskShift = 0x2,
        IDC_MaxVTableOffsetPlusOne = 0x1000,
    };

    DispatchCellInfo GetDispatchCellInfo()
    {
        // Capture m_pCache into a local for safe access (this is a volatile read of a value that may be
        // modified on another thread while this function is executing.)
        UIntTarget cachePointerValue = m_pCache;
        DispatchCellInfo cellInfo;

        if ((cachePointerValue < IDC_MaxVTableOffsetPlusOne) && ((cachePointerValue & IDC_CachePointerMask) == IDC_CachePointerPointsAtCache))
        {
            cellInfo.VTableOffset = (uint32_t)cachePointerValue;
            cellInfo.CellType = DispatchCellType::VTableOffset;
            cellInfo.HasCache = 1;
            return cellInfo;
        }

        // If there is a real cache pointer, grab the data from there.
        if ((cachePointerValue & IDC_CachePointerMask) == IDC_CachePointerPointsAtCache)
        {
            return ((InterfaceDispatchCacheHeader*)cachePointerValue)->GetDispatchCellInfo();
        }

        // Otherwise, walk to cell with Flags and Slot field

        // The slot number/flags for a dispatch cell is encoded once per run of DispatchCells
        // The run is terminated by having an dispatch cell with a null stub pointer.
        const InterfaceDispatchCell* currentCell = this;
        while (currentCell->m_pStub != 0)
        {
            currentCell = currentCell + 1;
        }
        UIntTarget cachePointerValueFlags = currentCell->m_pCache;

        DispatchCellType cellType = (DispatchCellType)(cachePointerValueFlags >> 16);
        cellInfo.CellType = cellType;

        if (cellType == DispatchCellType::InterfaceAndSlot)
        {
            cellInfo.InterfaceSlot = (uint16_t)cachePointerValueFlags;

            switch (cachePointerValue & IDC_CachePointerMask)
            {
            case IDC_CachePointerIsInterfacePointerOrMetadataToken:
                cellInfo.InterfaceType = (EEType*)(cachePointerValue & ~IDC_CachePointerMask);
                break;

            case IDC_CachePointerIsInterfaceRelativePointer:
            case IDC_CachePointerIsIndirectedInterfaceRelativePointer:
	            {
					UIntTarget interfacePointerValue = (UIntTarget)&m_pCache + (int32_t)cachePointerValue;
					interfacePointerValue &= ~IDC_CachePointerMask;

                    if ((cachePointerValue & IDC_CachePointerMask) == IDC_CachePointerIsInterfaceRelativePointer)
                    {
                        cellInfo.InterfaceType = (EEType*)interfacePointerValue;
                    }
                    else
                    {
                        cellInfo.InterfaceType = *(EEType**)interfacePointerValue;
                    }
	            }
                break;
            }
        }
        else
        {
            cellInfo.MetadataToken = (uint32_t)(cachePointerValue >> IDC_CachePointerMaskShift);
        }

        return cellInfo;
    }

    static bool IsCache(UIntTarget value)
    {
        if (((value & IDC_CachePointerMask) != 0) || (value < IDC_MaxVTableOffsetPlusOne))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    InterfaceDispatchCacheHeader* GetCache() const
    {
        // Capture m_pCache into a local for safe access (this is a volatile read of a value that may be
       // modified on another thread while this function is executing.)
        UIntTarget cachePointerValue = m_pCache;
        if (IsCache(cachePointerValue))
        {
            return (InterfaceDispatchCacheHeader*)cachePointerValue;
        }
        else
        {
            return 0;
        }
    }
};

#endif // FEATURE_CACHED_INTERFACE_DISPATCH