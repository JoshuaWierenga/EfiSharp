#ifndef __PALREDHALKINLINE_H__
#define __PALREDHALKINLINE_H__

#include "../CommonMacros.h"
#include "../unix/no_sal2.h"

#include "../../../EfiSharp.libc/internal/include/stdint.h"

//TODO Remove this file if it only contains these two functions without modification
EXTERN_C int64_t _InterlockedCompareExchange64(int64_t volatile*, int64_t, int64_t);
#pragma intrinsic(_InterlockedCompareExchange64)
FORCEINLINE int64_t PalInterlockedCompareExchange64(_Inout_ int64_t volatile* pDst, int64_t iValue, int64_t iComparand)
{
    return _InterlockedCompareExchange64(pDst, iValue, iComparand);
}

#if defined(HOST_AMD64) || defined(HOST_ARM64)
EXTERN_C uint8_t _InterlockedCompareExchange128(int64_t volatile*, int64_t, int64_t, int64_t*);
#pragma intrinsic(_InterlockedCompareExchange128)
FORCEINLINE uint8_t PalInterlockedCompareExchange128(_Inout_ int64_t volatile* pDst, int64_t iValueHigh, int64_t iValueLow, int64_t* pComparandAndResult)
{
    return _InterlockedCompareExchange128(pDst, iValueHigh, iValueLow, pComparandAndResult);
}
#endif // HOST_AMD64

#endif //__PALREDHALKINLINE_H_
