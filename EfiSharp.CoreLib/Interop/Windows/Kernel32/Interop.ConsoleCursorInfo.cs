// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_CURSOR_INFO
        {
            internal int dwSize;
            internal BOOL bVisible;
        }

        //TODO Add GeneratedDllImportAttribute
        //[GeneratedDllImport(Libraries.Kernel32, SetLastError = true)]
        [DllImport(Libraries.Kernel32)]
        //internal static partial bool GetConsoleCursorInfo(IntPtr hConsoleOutput, out CONSOLE_CURSOR_INFO cci);
        internal static extern bool GetConsoleCursorInfo(IntPtr hConsoleOutput, out CONSOLE_CURSOR_INFO cci);

        //TODO Add GeneratedDllImportAttribute
        //[GeneratedDllImport(Libraries.Kernel32, SetLastError = true)]
        [DllImport(Libraries.Kernel32)]
        //internal static partial bool SetConsoleCursorInfo(IntPtr hConsoleOutput, ref CONSOLE_CURSOR_INFO cci);
        internal static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, ref CONSOLE_CURSOR_INFO cci);
    }
}
