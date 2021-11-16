// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        //TODO Use CoreLib version
        internal static partial class FileAttributes
        {
            internal const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        }
    }
}