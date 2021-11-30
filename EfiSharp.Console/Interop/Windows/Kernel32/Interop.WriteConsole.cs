﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        //TODO Add GeneratedDllImportAttribute
        //[GeneratedDllImport(Libraries.Kernel32, EntryPoint = "WriteConsoleW", CharSet = CharSet.Unicode, SetLastError = true)]
        [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, EntryPoint = "WriteConsoleW")]
        //internal static unsafe partial bool WriteConsole(
        internal static extern unsafe bool WriteConsole(
            IntPtr hConsoleOutput,
            byte* lpBuffer,
            int nNumberOfCharsToWrite,
            out int lpNumberOfCharsWritten,
            IntPtr lpReservedMustBeNull);
    }
}
