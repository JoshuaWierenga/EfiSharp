// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

#ifndef __COMMONMACROS_H__
#define __COMMONMACROS_H__

#include "rhassert.h"

#define EXTERN_C extern "C"

#if defined(HOST_X86) && !defined(HOST_UNIX)
#define FASTCALL __fastcall
#else
#define FASTCALL
#endif

#define REDHAWK_API
#define REDHAWK_CALLCONV FASTCALL

#ifndef offsetof
#define offsetof(s,m)   (uintptr_t)( (intptr_t)&reinterpret_cast<const volatile char&>((((s *)0)->m)) )
#endif // offsetof

#ifndef FORCEINLINE
#define FORCEINLINE __forceinline
#endif

//
// Define an unmanaged function called from managed code that needs to execute in co-operative GC mode. (There
// should be very few of these, most such functions will be simply p/invoked).
//
#define COOP_PINVOKE_HELPER(_rettype, _method, _args) EXTERN_C REDHAWK_API _rettype REDHAWK_CALLCONV _method _args

#ifndef C_ASSERT
#define C_ASSERT(e) static_assert(e, #e)
#endif // C_ASSERT

#define UNREFERENCED_PARAMETER(P)          (void)(P)

#endif // __COMMONMACROS_H__