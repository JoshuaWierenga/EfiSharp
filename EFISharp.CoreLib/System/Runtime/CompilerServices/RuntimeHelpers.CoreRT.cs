// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    public static partial class RuntimeHelpers
    {
        [Intrinsic]
        public static bool IsReferenceOrContainsReferences<T>()
        {
            EETypePtr pEEType = EETypePtr.EETypePtrOf<T>();
            return !pEEType.IsValueType || pEEType.HasPointers;
        }

        //TODO Add Nullable
        //public static new bool Equals(object? o1, object? o2)
        public static new bool Equals(object o1, object o2)
        {
            if (o1 == o2)
                return true;

            if ((o1 == null) || (o2 == null))
                return false;

            // If it's not a value class, don't compare by value
            unsafe
            {
                if (!o1.EETypePtr.IsValueType)
                    return false;

                // Make sure they are the same type.
                if (o1.EETypePtr != o2.EETypePtr)
                    return false;
            }

            return RuntimeImports.RhCompareObjectContentsAndPadding(o1, o2);
        }

        public static int OffsetToStringData
        {
            // This offset is baked in by string indexer intrinsic, so there is no harm
            // in getting it baked in here as well.
            [System.Runtime.Versioning.NonVersionable]
            get =>
                // Number of bytes from the address pointed to by a reference to
                // a String to the first 16-bit character in the String.  Skip
                // over the MethodTable pointer, & String
                // length.  Of course, the String reference points to the memory
                // after the sync block, so don't count that.
                // This property allows C#'s fixed statement to work on Strings.
                // On 64 bit platforms, this should be 12 (8+4) and on 32 bit 8 (4+4).
#if TARGET_64BIT
                12;
#else // 32
                8;
#endif // TARGET_64BIT

        }

    }

    // CLR arrays are laid out in memory as follows (multidimensional array bounds are optional):
    // [ sync block || pMethodTable || num components || MD array bounds || array data .. ]
    //                 ^               ^                 ^                  ^ returned reference
    //                 |               |                 \-- ref Unsafe.As<RawArrayData>(array).Data
    //                 \-- array       \-- ref Unsafe.As<RawData>(array).Data
    // The BaseSize of an array includes all the fields before the array data,
    // including the sync block and method table. The reference to RawData.Data
    // points at the number of components, skipping over these two pointer-sized fields.
    [StructLayout(LayoutKind.Sequential)]
    internal class RawArrayData
    {
        public uint Length; // Array._numComponents padded to IntPtr
#if TARGET_64BIT
        public uint Padding;
#endif
        public byte Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class RawData
    {
        public byte Data;
    }
}
