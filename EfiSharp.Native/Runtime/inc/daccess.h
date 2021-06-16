// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.
//*****************************************************************************
// File: daccess.h
//
// Support for external access of runtime data structures.  These
// macros and templates hide the details of pointer and data handling
// so that data structures and code can be compiled to work both
// in-process and through a special memory access layer.
//
// This code assumes the existence of two different pieces of code,
// the target, the runtime code that is going to be examined, and
// the host, the code that's doing the examining.  Access to the
// target is abstracted so the target may be a live process on the
// same machine, a live process on a different machine, a dump file
// or whatever.  No assumptions should be made about accessibility
// of the target.
//
// This code assumes that the data in the target is static.  Any
// time the target's data changes the interfaces must be reset so
// that potentially stale data is discarded.
//
// This code is intended for read access and there is no
// way to write data back currently.
//
// DAC-ized code:
// - is read-only (non-invasive). So DACized codepaths can not trigger a GC.
// - has no Thread* object.  In reality, DAC-ized codepaths are
//   ReadProcessMemory calls from out-of-process. Conceptually, they
//   are like a pure-native (preemptive) thread.
////
// This means that in particular, you cannot DACize a GCTRIGGERS function.
// Neither can you DACize a function that throws if this will involve
// allocating a new exception object. There may be
// exceptions to these rules if you can guarantee that the DACized
// part of the code path cannot cause a garbage collection (see
// EditAndContinueModule::ResolveField for an example).
// If you need to DACize a function that may trigger
// a GC, it is probably best to refactor the function so that the DACized
// part of the code path is in a separate function. For instance,
// functions with GetOrCreate() semantics are hard to DAC-ize because
// they the Create portion is inherently invasive. Instead, consider refactoring
// into a GetOrFail() function that DAC can call; and then make GetOrCreate()
// a wrapper around that.

#ifndef __daccess_h__ 
#define __daccess_h__

#include "type_traits.hpp"

#ifdef DACCESS_COMPILE

#if defined(TARGET_AMD64) || defined(TARGET_ARM64)
typedef uint64_t UIntTarget;
#elif defined(TARGET_X86)
typedef uint32_t UIntTarget;
#elif defined(TARGET_ARM)
typedef uint32_t UIntTarget;
#else
#error unexpected target architecture
#endif

// Define TADDR as a non-pointer value so use of it as a pointer
// will not work properly.  Define it as unsigned so
// pointer comparisons aren't affected by sign.
// This requires special casting to ULONG64 to sign-extend if necessary.
// XXX drewb - Cheating right now by not supporting cross-plat.
typedef UIntTarget TADDR;

// TSIZE_T used for counts or ranges that need to span the size of a
// target pointer.  For cross-plat, this may be different than SIZE_T
// which reflects the host pointer size.
typedef UIntTarget TSIZE_T;

void* DacInstantiateTypeByAddress(TADDR addr, uint32_t size, bool throwEx);

TADDR    DacGetTargetAddrForHostAddr(const void* ptr, bool throwEx);

// Report a region of memory to the debugger
void    DacEnumMemoryRegion(TADDR addr, TSIZE_T size, bool fExpectSuccess = true);

// Base pointer wrapper which provides common behavior.
class __TPtrBase
{
public:
	__TPtrBase()
	{
		// Make uninitialized pointers obvious.
		m_addr = (TADDR)-1;
	}
	explicit __TPtrBase(TADDR addr)
	{
		m_addr = addr;
	}
	bool operator!() const
	{
		return m_addr == 0;
	}
	// We'd like to have an implicit conversion to bool here since the C++
	// standard says all pointer types are implicitly converted to bool.
	// Unfortunately, that would cause ambiguous overload errors for uses
	// of operator== and operator!=.  Instead callers will have to compare
	// directly against NULL.

	bool operator==(TADDR addr) const
	{
		return m_addr == addr;
	}
	bool operator!=(TADDR addr) const
	{
		return m_addr != addr;
	}
	bool operator<(TADDR addr) const
	{
		return m_addr < addr;
	}
	bool operator>(TADDR addr) const
	{
		return m_addr > addr;
	}
	bool operator<=(TADDR addr) const
	{
		return m_addr <= addr;
	}
	bool operator>=(TADDR addr) const
	{
		return m_addr >= addr;
	}

	TADDR GetAddr(void) const
	{
		return m_addr;
	}
	TADDR SetAddr(TADDR addr)
	{
		m_addr = addr;
		return addr;
	}

protected:
	TADDR m_addr;
};

// Adds comparison operations
// Its possible we just want to merge these into __TPtrBase, but SPtr isn't comparable with
// other types right now and I would rather stay conservative
class __ComparableTPtrBase : public __TPtrBase
{
protected:
	__ComparableTPtrBase(void) : __TPtrBase()
	{}

	explicit __ComparableTPtrBase(TADDR addr) : __TPtrBase(addr)
	{}

public:
	bool operator==(const __ComparableTPtrBase& ptr) const
	{
		return m_addr == ptr.m_addr;
	}
	bool operator!=(const __ComparableTPtrBase& ptr) const
	{
		return !operator==(ptr);
	}
	bool operator<(const __ComparableTPtrBase& ptr) const
	{
		return m_addr < ptr.m_addr;
	}
	bool operator>(const __ComparableTPtrBase& ptr) const
	{
		return m_addr > ptr.m_addr;
	}
	bool operator<=(const __ComparableTPtrBase& ptr) const
	{
		return m_addr <= ptr.m_addr;
	}
	bool operator>=(const __ComparableTPtrBase& ptr) const
	{
		return m_addr >= ptr.m_addr;
	}
};

//
// Computes (taBase + (dwIndex * dwElementSize()), with overflow checks.
//
// Arguments:
//     taBase          the base TADDR value
//     dwIndex         the index of the offset
//     dwElementSize   the size of each element (to multiply the offset by)
//
// Return value:
//     The resulting TADDR, or throws CORDB_E_TARGET_INCONSISTENT on overlow.
//
// Notes:
//     The idea here is that overflows during address arithmetic suggest that we're operating on corrupt
//     pointers.  It helps to improve reliability to detect the cases we can (like overflow) and fail.  Note
//     that this is just a heuristic, not a security measure.  We can't trust target data regardless -
//     failing on overflow is just one easy case of corruption to detect.  There is no need to use checked
//     arithmetic everywhere in the DAC infrastructure, this is intended just for the places most likely to
//     help catch bugs (eg. __DPtr::operator[]).
//
inline TADDR DacTAddrOffset(TADDR taBase, TSIZE_T dwIndex, TSIZE_T dwElementSize)
{
#ifdef DAC_CLR_ENVIRONMENT
	ClrSafeInt<TADDR> t(taBase);
	t += ClrSafeInt<TSIZE_T>(dwIndex) * ClrSafeInt<TSIZE_T>(dwElementSize);
	if (t.IsOverflow())
	{
		// Pointer arithmetic overflow - probably due to corrupt target data
		//DacError(CORDBG_E_TARGET_INCONSISTENT);
		DacError(E_FAIL);
	}
	return t.Value();
#else // TODO: port safe math
	return taBase + (dwIndex * dwElementSize);
#endif
}

// Pointer wrapper base class for various forms of normal data.
// This has the common functionality between __DPtr and __ArrayDPtr.
// The DPtrType type parameter is the actual derived type in use.  This is necessary so that
// inhereted functions preserve exact return types.
template<typename type, typename DPtrType>
class __DPtrBase : public __ComparableTPtrBase
{
public:
	typedef type _Type;
	typedef type* _Ptr;

protected:
	// Constructors
	// All protected - this type should not be used directly - use one of the derived types instead.
	__DPtrBase< type, DPtrType >(void) : __ComparableTPtrBase()
	{}

	explicit __DPtrBase< type, DPtrType >(TADDR addr) : __ComparableTPtrBase(addr)
	{}

	explicit __DPtrBase(__TPtrBase addr)
	{
		m_addr = addr.GetAddr();
	}
	explicit __DPtrBase(type const* host)
	{
		m_addr = DacGetTargetAddrForHostAddr(host, true);
	}

public:
	DPtrType& operator=(const __TPtrBase& ptr)
	{
		m_addr = ptr.GetAddr();
		return DPtrType(m_addr);
	}
	DPtrType& operator=(TADDR addr)
	{
		m_addr = addr;
		return DPtrType(m_addr);
	}

	type& operator*(void) const
	{
		return *(type*)DacInstantiateTypeByAddress(m_addr, sizeof(type), true);
	}

	
	using __ComparableTPtrBase::operator==;
	using __ComparableTPtrBase::operator!=;
	using __ComparableTPtrBase::operator<;
	using __ComparableTPtrBase::operator>;
	using __ComparableTPtrBase::operator<=;
	using __ComparableTPtrBase::operator>=;
	bool operator==(TADDR addr) const
	{
		return m_addr == addr;
	}
	bool operator!=(TADDR addr) const
	{
		return m_addr != addr;
	}

	// Array index operator
	// we want an operator[] for all possible numeric types (rather than rely on
	// implicit numeric conversions on the argument) to prevent ambiguity with
	// DPtr's implicit conversion to type* and the built-in operator[].
	// @dbgtodo rbyers: we could also use this technique to simplify other operators below.
	template<typename indexType>
	type& operator[](indexType index)
	{
		// Compute the address of the element.
		TADDR elementAddr;
		if (index >= 0)
		{
			elementAddr = DacTAddrOffset(m_addr, index, sizeof(type));
		}
		else
		{
			// Don't bother trying to do overflow checking for negative indexes - they are rare compared to
			// positive ones.  ClrSafeInt doesn't support signed datatypes yet (although we should be able to add it
			// pretty easily).
			elementAddr = m_addr + index * sizeof(type);
		}

		// Marshal over a single instance and return a reference to it.
		return *(type*)DacInstantiateTypeByAddress(elementAddr, sizeof(type), true);
	}

	template<typename indexType>
	type const& operator[](indexType index) const
	{
		return (*const_cast<__DPtrBase*>(this))[index];
	}

	//-------------------------------------------------------------------------
	// operator+

	DPtrType operator+(unsigned short val)
	{
		return DPtrType(DacTAddrOffset(m_addr, val, sizeof(type)));
	}
	DPtrType operator+(short val)
	{
		return DPtrType(m_addr + val * sizeof(type));
	}
	// size_t is unsigned int on Win32, so we need
	// to ifdef here to make sure the unsigned int
	// and size_t overloads don't collide.  size_t
	// is marked __w64 so a simple unsigned int
	// will not work on Win32, it has to be size_t.
	DPtrType operator+(size_t val)
	{
		return DPtrType(DacTAddrOffset(m_addr, val, sizeof(type)));
	}
#if (!defined (HOST_X86) && !defined(_SPARC_) && !defined(HOST_ARM)) || (defined(HOST_X86) && defined(__APPLE__))
	DPtrType operator+(unsigned int val)
	{
		return DPtrType(DacTAddrOffset(m_addr, val, sizeof(type)));
	}
#endif // (!defined (HOST_X86) && !defined(_SPARC_) && !defined(HOST_ARM)) || (defined(HOST_X86) && defined(__APPLE__))
	DPtrType operator+(int val)
	{
		return DPtrType(m_addr + val * sizeof(type));
	}
#ifndef TARGET_UNIX // for now, everything else is 32 bit
	DPtrType operator+(unsigned long val)
	{
		return DPtrType(DacTAddrOffset(m_addr, val, sizeof(type)));
	}
	DPtrType operator+(long val)
	{
		return DPtrType(m_addr + val * sizeof(type));
	}
#endif // !TARGET_UNIX // for now, everything else is 32 bit
#if !defined(HOST_ARM) && !defined(HOST_X86)
	DPtrType operator+(intptr_t val)
	{
		return DPtrType(m_addr + val * sizeof(type));
	}
#endif

	//-------------------------------------------------------------------------
	// operator-

	DPtrType operator-(unsigned short val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
	DPtrType operator-(short val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
	// size_t is unsigned int on Win32, so we need
	// to ifdef here to make sure the unsigned int
	// and size_t overloads don't collide.  size_t
	// is marked __w64 so a simple unsigned int
	// will not work on Win32, it has to be size_t.
	DPtrType operator-(size_t val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
	DPtrType operator-(signed __int64 val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
#if !defined (HOST_X86) && !defined(_SPARC_) && !defined(HOST_ARM)
	DPtrType operator-(unsigned int val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
#endif // !defined (HOST_X86) && !defined(_SPARC_) && !defined(HOST_ARM)
	DPtrType operator-(int val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
#ifdef _MSC_VER // for now, everything else is 32 bit
	DPtrType operator-(unsigned long val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
	DPtrType operator-(long val)
	{
		return DPtrType(m_addr - val * sizeof(type));
	}
#endif // _MSC_VER // for now, everything else is 32 bit
	size_t operator-(const DPtrType& val)
	{
		return (size_t)((m_addr - val.m_addr) / sizeof(type));
	}

	//-------------------------------------------------------------------------

	DPtrType& operator+=(size_t val)
	{
		m_addr += val * sizeof(type);
		return static_cast<DPtrType&>(*this);
	}
	DPtrType& operator-=(size_t val)
	{
		m_addr -= val * sizeof(type);
		return static_cast<DPtrType&>(*this);
	}

	DPtrType& operator++()
	{
		m_addr += sizeof(type);
		return static_cast<DPtrType&>(*this);
	}
	DPtrType& operator--()
	{
		m_addr -= sizeof(type);
		return static_cast<DPtrType&>(*this);
	}
	DPtrType operator++(int postfix)
	{
		UNREFERENCED_PARAMETER(postfix);
		DPtrType orig = DPtrType(*this);
		m_addr += sizeof(type);
		return orig;
	}
	DPtrType operator--(int postfix)
	{
		UNREFERENCED_PARAMETER(postfix);
		DPtrType orig = DPtrType(*this);
		m_addr -= sizeof(type);
		return orig;
	}

	bool IsValid(void) const
	{
		return m_addr &&
			DacInstantiateTypeByAddress(m_addr, sizeof(type),
				false) != NULL;
	}
	void EnumMem(void) const
	{
		DacEnumMemoryRegion(m_addr, sizeof(type));
	}
};

// Pointer wrapper for objects which are just plain data
// and need no special handling.
template<typename type>
class __DPtr : public __DPtrBase<type, __DPtr<type> >
{
#ifdef __GNUC__
protected:
	//there seems to be a bug in GCC's inference logic.  It can't find m_addr.
	using __DPtrBase<type, __DPtr<type> >::m_addr;
#endif // __GNUC__
public:
	// constructors - all chain to __DPtrBase constructors
	__DPtr< type >(void) : __DPtrBase<type, __DPtr<type> >() {}
	__DPtr< type >(TADDR addr) : __DPtrBase<type, __DPtr<type> >(addr) {}

	// construct const from non-const
	typedef typename type_traits::remove_const<type>::type mutable_type;
	__DPtr< type >(__DPtr<mutable_type> const& rhs) : __DPtrBase<type, __DPtr<type> >(rhs.GetAddr()) {}

	explicit __DPtr< type >(__TPtrBase addr) : __DPtrBase<type, __DPtr<type> >(addr) {}
	explicit __DPtr< type >(type const* host) : __DPtrBase<type, __DPtr<type> >(host) {}

	operator type* () const
	{
		return (type*)DacInstantiateTypeByAddress(m_addr, sizeof(type), true);
	}
	type* operator->() const
	{
		return (type*)DacInstantiateTypeByAddress(m_addr, sizeof(type), true);
	}
};

#define DPTR(type) __DPtr< type >

// Constructs an arbitrary data instance for a piece of
// memory in the target.
#define PTR_READ(addr, size) \
    DacInstantiateTypeByAddress(addr, size, true)

#else // !DACCESS_COMPILE

//
// This version of the macros turns into normal pointers
// for unmodified in-proc compilation.

#define DPTR(type) type*

#endif // !DACCESS_COMPILE

typedef uint8_t               Code;
typedef DPTR(Code)          PTR_Code;

#endif // !__daccess_h__