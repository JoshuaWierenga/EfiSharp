#ifndef __PALREDHALKINLINE_H__
#define __PALREDHALKINLINE_H__

#include "../CommonMacros.h"

//TODO Remove this file and use windows version if it only contains this function without modification
#if defined(HOST_AMD64) || defined(HOST_ARM64)
EXTERN_C uint8_t _InterlockedCompareExchange128(int64_t volatile*, int64_t, int64_t, int64_t*);
#pragma intrinsic(_InterlockedCompareExchange128)
FORCEINLINE uint8_t PalInterlockedCompareExchange128(_Inout_ int64_t volatile* pDst, int64_t iValueHigh, int64_t iValueLow, int64_t* pComparandAndResult)
{
    return _InterlockedCompareExchange128(pDst, iValueHigh, iValueLow, pComparandAndResult);
}
#endif // HOST_AMD64

#endif //__PALREDHALKINLINE_H_
