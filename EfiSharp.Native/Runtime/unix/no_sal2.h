// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

/***
*       no_sal2.h - renders the SAL annotations for documenting APIs harmless.
*
*
*Purpose:
*       sal.h provides a set of SAL2 annotations to describe how a function uses its
*       parameters - the assumptions it makes about them, and the guarantees it makes
*       upon finishing. This file redefines all those annotation macros to be harmless.
*       It is designed for use in down-level build environments where the tooling may
*       be unhappy with the standard SAL2 macro definitions.
*
*       [Public]
*
*
****/

#ifndef _NO_SAL_2_H_
#define _NO_SAL_2_H_

#undef _Inout_
#define _Inout_

#endif /* _NO_SAL_2_H_ */