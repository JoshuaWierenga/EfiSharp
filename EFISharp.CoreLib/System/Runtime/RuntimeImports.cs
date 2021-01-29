﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.CompilerServices;

namespace System.Runtime
{
    // CONTRACT with Runtime
    // This class lists all the static methods that the redhawk runtime exports to a class library
    // These are not expected to change much but are needed by the class library to implement its functionality
    //
    //      The contents of this file can be modified if needed by the class library
    //      E.g., the class and methods are marked internal assuming that only the base class library needs them
    //            but if a class library wants to factor differently (such as putting the GCHandle methods in an
    //            optional library, those methods can be moved to a different file/namespace/dll

    public static class RuntimeImports
    {
        private const string RuntimeLibrary = "*";

        //
        // calls to runtime for allocation
        // These calls are needed in types which cannot use "new" to allocate and need to do it manually
        //
        //[MethodImpl(MethodImplOptions.InternalCall)]
        //[RuntimeImport(RuntimeLibrary, "RhpFallbackFailFast")]
        //Only references to this function I can find lead to RaiseFailFastException within windows, how is this handled normally?
        internal static unsafe void RhpFallbackFailFast() { }

        //
        // Interlocked helpers
        //
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpLockCmpXchg32")]
        internal static extern int InterlockedCompareExchange(ref int location1, int value, int comparand);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpMemoryBarrier")]
        internal static extern void MemoryBarrier();

    }
}