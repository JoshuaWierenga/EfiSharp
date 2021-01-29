﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime;

using EEType = Internal.Runtime.EEType;

using System.Runtime.InteropServices;

namespace System
{
    // CONTRACT with Runtime
    // The Array type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type int

    public partial class Array
    {
        // This field should be the first field in Array as the runtime/compilers depend on it
        internal int _numComponents;

        public int Length
        {
            get
            {
                // NOTE: The compiler has assumptions about the implementation of this method.
                // Changing the implementation here (or even deleting this) will NOT have the desired impact
                return _numComponents;
            }
        }

        // This is the classlib-provided "get array eetype" function that will be invoked whenever the runtime
        // needs to know the base type of an array.
        [RuntimeExport("GetSystemArrayEEType")]
        private static unsafe EEType* GetSystemArrayEEType()
        {
            return EETypePtr.EETypePtrOf<Array>().ToPointer();
        }
    }

    // To accommodate class libraries that wish to implement generic interfaces on arrays, all class libraries
    // are now required to provide an Array<T> class that derives from Array.
    internal class Array<T> : Array
    {
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class RawArrayData
    {
        public uint Length; // Array._numComponents padded to IntPtr
#if BIT64
        public uint Padding;
#endif
        public byte Data;
    }
}