// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

//#include "common.h"
//#include "CommonTypes.h"
//#include "CommonMacros.h"
//#include "rhassert.h"

#include <cstdint>
#include "../../runtimelab/src/coreclr/nativeaot/Runtime/CommonMacros.h"

//
// Floating point and 64-bit integer math helpers.
//

EXTERN_C REDHAWK_API uint64_t REDHAWK_CALLCONV RhpDbl2ULng(double val)
{
    return((uint64_t)val);
}