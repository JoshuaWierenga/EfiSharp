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
        //[GeneratedDllImport(Libraries.Kernel32, EntryPoint = "ReadConsoleW", CharSet = CharSet.Unicode, SetLastError = true)]
        [DllImport(Libraries.Kernel32, EntryPoint = "ReadConsoleW")]
        //internal static unsafe partial bool ReadConsole(
        internal static extern unsafe bool ReadConsole(
            IntPtr hConsoleInput,
            byte* lpBuffer,
            int nNumberOfCharsToRead,
            out int lpNumberOfCharsRead,
            IntPtr pInputControl);
    }
}
