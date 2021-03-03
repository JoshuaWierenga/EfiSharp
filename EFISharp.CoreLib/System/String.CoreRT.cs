// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

using Internal.Runtime.CompilerServices;

namespace System
{
    // This class is marked EagerStaticClassConstruction because it's nice to have this
    // eagerly constructed to avoid the cost of defered ctors. I can't imagine any app that doesn't use string
    //
    [EagerStaticClassConstruction]
    public partial class String
    {
        [Intrinsic]
        public static readonly string Empty = "";

        //TODO Add IndexerName
        //[System.Runtime.CompilerServices.IndexerName("Chars")]
        public unsafe char this[int index]
        {
            [Intrinsic]
            get
            {
                if ((uint)index >= _stringLength)
                    //TODO Add ThrowHelper
                    //ThrowHelper.ThrowIndexOutOfRangeException();
                    throw new IndexOutOfRangeException();
                return Unsafe.Add(ref _firstChar, index);
            }
        }

        public int Length
        {
            [Intrinsic]
            get => _stringLength;
        }

        internal static string FastAllocateString(int length)
        {
            // We allocate one extra char as an interop convenience so that our strings are null-
            // terminated, however, we don't pass the extra +1 to the string allocation because the base
            // size of this object includes the _firstChar field.
            //TODO Add RhNewString, this method should work for now however and is exactly what the portable runtime does
            //See https://github.com/dotnet/runtimelab/blob/848031f/src/coreclr/nativeaot/Runtime/portable.cpp#L171-L176
            //string newStr = RuntimeImports.RhNewString(EETypePtr.EETypePtrOf<string>(), length);
            object data = InternalCalls.RhpNewArray(EETypePtr.EETypePtrOf<string>(), length);
            string newStr = Unsafe.As<object, string>(ref data);

            Debug.Assert(newStr._stringLength == length);
            return newStr;
        }
    }
}