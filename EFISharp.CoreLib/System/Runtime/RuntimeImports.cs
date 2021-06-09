// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Internal.Runtime;

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

    //TODO: Use Test.CoreLib version?
    public static class RuntimeImports
    {
        private const string RuntimeLibrary = "*";

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhCompareObjectContentsAndPadding")]
        internal static extern bool RhCompareObjectContentsAndPadding(object obj1, object obj2);

        //
        // calls to runtime for type equality checks
        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhTypeCast_AreTypesEquivalent")]
        private static extern unsafe bool AreTypesEquivalent(EEType* pType1, EEType* pType2);

        internal static unsafe bool AreTypesEquivalent(EETypePtr pType1, EETypePtr pType2)
            => AreTypesEquivalent(pType1.ToPointer(), pType2.ToPointer());

        //
        // calls to runtime for allocation
        // These calls are needed in types which cannot use "new" to allocate and need to do it manually
        //
        // calls to runtime for allocation
        //
        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhNewObject")]
        private static extern unsafe object RhNewObject(EEType* pEEType);

        internal static unsafe object RhNewObject(EETypePtr pEEType)
            => RhNewObject(pEEType.ToPointer());

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhNewArray")]
        private static extern unsafe Array RhNewArray(EEType* pEEType, int length);

        internal static unsafe Array RhNewArray(EETypePtr pEEType, int length)
            => RhNewArray(pEEType.ToPointer(), length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhNewString")]
        internal static extern unsafe string RhNewString(EEType* pEEType, int length);

        internal static unsafe string RhNewString(EETypePtr pEEType, int length)
            => RhNewString(pEEType.ToPointer(), length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhBox")]
        private static extern unsafe object RhBox(EEType* pEEType, ref byte data);

        internal static unsafe object RhBox(EETypePtr pEEType, ref byte data)
            => RhBox(pEEType.ToPointer(), ref data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpFallbackFailFast")]
        internal static extern unsafe void RhpFallbackFailFast();


        //
        // Interlocked helpers
        //
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpLockCmpXchg32")]
        internal static extern int InterlockedCompareExchange(ref int location1, int value, int comparand);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpMemoryBarrier")]
        internal static extern void MemoryBarrier();

        //TODO Figure out why this works without adding sqrt to libc
        [Intrinsic]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "sqrt")]
        internal static extern double sqrt(double x);

        [Intrinsic]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "modf")]
        internal static extern unsafe double modf(double x, double* intptr);

        //TODO Add ExactSpelling to DllImportAttribute
        //[DllImport(RuntimeImports.RuntimeLibrary, ExactSpelling = true)]
        [DllImport(RuntimeLibrary)]
        internal static extern unsafe void* memmove(byte* dmem, byte* smem, nuint size);

        //TODO Add ExactSpelling to DllImportAttribute
        //[DllImport(RuntimeImports.RuntimeLibrary, ExactSpelling = true)]
        [DllImport(RuntimeLibrary)]
        internal static extern unsafe void* memset(byte* mem, int value, nuint size);


        internal static RhCorElementTypeInfo GetRhCorElementTypeInfo(CorElementType elementType)
        {
            return RhCorElementTypeInfo.GetRhCorElementTypeInfo(elementType);
        }

        internal struct RhCorElementTypeInfo
        {
            public RhCorElementTypeInfo(byte log2OfSize, ushort widenMask, bool isPrimitive = false)
            {
                _log2OfSize = log2OfSize;
                RhCorElementTypeInfoFlags flags = RhCorElementTypeInfoFlags.IsValid;
                if (isPrimitive)
                    flags |= RhCorElementTypeInfoFlags.IsPrimitive;
                _flags = flags;
                _widenMask = widenMask;
            }

            public bool IsPrimitive
            {
                get { return 0 != (_flags & RhCorElementTypeInfoFlags.IsPrimitive); }
            }

            public bool IsFloat
            {
                get { return 0 != (_flags & RhCorElementTypeInfoFlags.IsFloat); }
            }

            public byte Log2OfSize
            {
                get { return _log2OfSize; }
            }

            //
            // This is a port of InvokeUtil::CanPrimitiveWiden() in the desktop runtime. This is used by various apis such as Array.SetValue()
            // and Delegate.DynamicInvoke() which allow value-preserving widenings from one primitive type to another.
            //
            public bool CanWidenTo(CorElementType targetElementType)
            {
                // Caller expected to ensure that both sides are primitive before calling us.
                Debug.Assert(this.IsPrimitive);
                Debug.Assert(GetRhCorElementTypeInfo(targetElementType).IsPrimitive);

                // Once we've asserted that the target is a primitive, we can also assert that it is >= ET_BOOLEAN.
                Debug.Assert(targetElementType >= CorElementType.ELEMENT_TYPE_BOOLEAN);
                byte targetElementTypeAsByte = (byte)targetElementType;
                ushort
                    mask = (ushort)(1 <<
                                     targetElementTypeAsByte
                        ); // This is expected to overflow on larger ET_I and ET_U - this is ok and anticipated.
                if (0 != (_widenMask & mask))
                    return true;
                return false;
            }


            internal static RhCorElementTypeInfo GetRhCorElementTypeInfo(CorElementType elementType)
            {
                //TODO Fix crashes from static array fields
                RhCorElementTypeInfo[] s_lookupTable = new RhCorElementTypeInfo[]
                {
                    // index = 0x0
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x1
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x2 = ELEMENT_TYPE_BOOLEAN   (W = BOOL)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 0, _widenMask = 0x0004,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x3 = ELEMENT_TYPE_CHAR      (W = U2, CHAR, I4, U4, I8, U8, R4, R8) (U2 == Char)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 1, _widenMask = 0x3f88,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x4 = ELEMENT_TYPE_I1        (W = I1, I2, I4, I8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 0, _widenMask = 0x3550,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x5 = ELEMENT_TYPE_U1        (W = CHAR, U1, I2, U2, I4, U4, I8, U8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 0, _widenMask = 0x3FE8,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x6 = ELEMENT_TYPE_I2        (W = I2, I4, I8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 1, _widenMask = 0x3540,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x7 = ELEMENT_TYPE_U2        (W = U2, CHAR, I4, U4, I8, U8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 1, _widenMask = 0x3F88,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x8 = ELEMENT_TYPE_I4        (W = I4, I8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 2, _widenMask = 0x3500,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x9 = ELEMENT_TYPE_U4        (W = U4, I8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 2, _widenMask = 0x3E00,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0xa = ELEMENT_TYPE_I8        (W = I8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 3, _widenMask = 0x3400,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0xb = ELEMENT_TYPE_U8        (W = U8, R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 3, _widenMask = 0x3800,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0xc = ELEMENT_TYPE_R4        (W = R4, R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 2, _widenMask = 0x3000,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive |
                                 RhCorElementTypeInfoFlags.IsFloat
                    },
                    // index = 0xd = ELEMENT_TYPE_R8        (W = R8)
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = 3, _widenMask = 0x2000,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive |
                                 RhCorElementTypeInfoFlags.IsFloat
                    },
                    // index = 0xe
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0xf
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x10
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x11
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x12
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x13
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x14
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x15
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x16
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x17
                    new RhCorElementTypeInfo {_log2OfSize = 0, _widenMask = 0x0000, _flags = 0},
                    // index = 0x18 = ELEMENT_TYPE_I
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = log2PointerSize, _widenMask = 0x0000,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                    // index = 0x19 = ELEMENT_TYPE_U
                    new RhCorElementTypeInfo
                    {
                        _log2OfSize = log2PointerSize, _widenMask = 0x0000,
                        _flags = RhCorElementTypeInfoFlags.IsValid | RhCorElementTypeInfoFlags.IsPrimitive
                    },
                };

                // The _lookupTable array only covers a subset of RhCorElementTypes, so we return a default
                // info when someone asks for an elementType which does not have an entry in the table.
                if ((int)elementType > s_lookupTable.Length)
                    return default(RhCorElementTypeInfo);

                RhCorElementTypeInfo result = s_lookupTable[(int) elementType];
                s_lookupTable.Free();
                return result;
            }

            private byte _log2OfSize;
            private RhCorElementTypeInfoFlags _flags;

            [Flags]
            private enum RhCorElementTypeInfoFlags : byte
            {
                IsValid = 0x01, // Set for all valid CorElementTypeInfo's
                IsPrimitive = 0x02, // Is it a primitive type (as defined by TypeInfo.IsPrimitive)
                IsFloat = 0x04, // Is it a floating point type
            }

            private ushort _widenMask;

#if TARGET_64BIT
            const byte log2PointerSize = 3;
#else
            private const byte log2PointerSize = 2;
#endif
        }
    }
}