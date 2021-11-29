// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        //TODO Add GeneratedDllImportAttribute
        //[GeneratedDllImport(Libraries.Kernel32, SetLastError = true)]
        [DllImport(Libraries.Kernel32)]
        //internal static partial bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, Interop.Kernel32.COORD size);
        internal static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, Interop.Kernel32.COORD size);
    }
}

