﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class BCrypt
    {
        internal const int BCRYPT_USE_SYSTEM_PREFERRED_RNG = 0x00000002;

        //TODO Add DllImportAttribute.CharSet
        //[DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode)]
        [DllImport(Libraries.BCrypt)]
        internal static extern unsafe NTSTATUS BCryptGenRandom(IntPtr hAlgorithm, byte* pbBuffer, int cbBuffer, int dwFlags);
    }
}