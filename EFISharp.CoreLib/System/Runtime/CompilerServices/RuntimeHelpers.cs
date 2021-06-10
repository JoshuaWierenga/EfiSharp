// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Reflection;

namespace System.Runtime.CompilerServices
{
    public static partial class RuntimeHelpers
    {
        internal static bool IsPrimitiveType(this CorElementType et)
            // COR_ELEMENT_TYPE_I1,I2,I4,I8,U1,U2,U4,U8,R4,R8,I,U,CHAR,BOOLEAN
            => ((1 << (int)et) & 0b_0011_0000_0000_0011_1111_1111_1100) != 0;
    }
}