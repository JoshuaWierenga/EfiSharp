// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

#ifndef _TARGETPTRS_H_
#define _TARGETPTRS_H_

#include "../../../EfiSharp.libc/internal/include/stdint.h"

#ifdef TARGET_AMD64
typedef uint64_t UIntTarget;
#elif defined(TARGET_X86)
typedef uint32_t UIntTarget;
#elif defined(TARGET_ARM)
typedef uint32_t UIntTarget;
#elif defined(TARGET_ARM64)
typedef uint64_t UIntTarget;
#elif defined(TARGET_WASM)
typedef uint32_t UIntTarget;
#else
#error unexpected target architecture
#endif

#endif // !_TARGETPTRS_H_