// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfiSharp;
using Internal.Runtime.CompilerServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public partial class String
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        //[DynamicDependency("Ctor(System.Char[],System.Int32,System.Int32)")]
        public extern String(char[] value, int startIndex, int length);

        private static string Ctor(char[] value, int startIndex, int length)
        {
            EETypePtr et = EETypePtr.EETypePtrOf<string>();

            unsafe
            {
                //Todo use Unsafe.Add/.AddByteOffset, currently, it fails if stateIndex is not 0
                char* start;
                fixed (char* pValue = value)
                {
                    start = pValue + startIndex;
                }

                object data = InternalCalls.RhpNewArray(et.ToPointer(), length);
                string s = Unsafe.As<object, string>(ref data);

                fixed (char* c = s)
                {
                    UefiApplication.SystemTable->BootServices->CopyMem(c, start, (nuint)length * sizeof(char));
                    c[length] = '\0';
                }

                return s;
            }
        }

        //[CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        //[DynamicDependency("Ctor(System.Char*,System.Int32,System.Int32)")]
        public extern unsafe String(char* ptr, int startIndex, int length);

        //TODO Merge into FastAllocateString and other constructors
        //From https://github.com/Michael-Kelley/RoseOS/blob/0cf31ff/CoreLib/System/String.cs#L60
        private static unsafe string Ctor(char* ptr, int startIndex, int length)
        {
            EETypePtr et = EETypePtr.EETypePtrOf<string>();

            char* start = ptr + startIndex;
            object data = InternalCalls.RhpNewArray(et.ToPointer(), length);
            string s = Unsafe.As<object, string>(ref data);

            fixed (char* c = s)
            {
                UefiApplication.SystemTable->BootServices->CopyMem(c, start, (nuint)length * sizeof(char));
                c[length] = '\0';
            }

            return s;
        }
    }
}
