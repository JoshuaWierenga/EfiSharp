// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    //TODO Use CoreLib version
    internal static partial class Kernel32
    {
        [DllImport(Libraries.Kernel32)]
        internal static extern unsafe int WideCharToMultiByte(
            uint CodePage, uint dwFlags,
            char* lpWideCharStr, int cchWideChar,
            byte* lpMultiByteStr, int cbMultiByte,
            IntPtr lpDefaultChar, IntPtr lpUsedDefaultChar);
    }
}
