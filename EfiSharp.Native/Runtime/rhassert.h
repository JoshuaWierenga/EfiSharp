// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

#if defined(_DEBUG) && !defined(DACCESS_COMPILE)

#else

#define ASSERT(expr)

#endif // __RHASSERT_H__