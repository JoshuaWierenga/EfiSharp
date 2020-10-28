// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
        private const string RuntimeLibrary = "[MRT]";

        //
        // calls to runtime for allocation
        // These calls are needed in types which cannot use "new" to allocate and need to do it manually
        //
        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpFallbackFailFast")]
        internal static extern unsafe void RhpFallbackFailFast();
    }
}