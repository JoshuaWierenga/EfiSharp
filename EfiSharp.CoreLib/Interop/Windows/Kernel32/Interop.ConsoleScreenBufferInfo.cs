// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System.Runtime.InteropServices;

internal static partial class Interop
{
    //TODO Use CoreLib version
    internal static partial class Kernel32
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_SCREEN_BUFFER_INFO
        {
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal short wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct COORD
        {
            internal short X;
            internal short Y;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        internal partial struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }
    }
}
