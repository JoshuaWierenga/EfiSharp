// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;
using System.Runtime.InteropServices;

//TODO Use CoreLib version
internal static partial class Interop
{
    internal const short KEY_EVENT = 1;

    // Windows's KEY_EVENT_RECORD
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct KeyEventRecord
    {
        internal BOOL keyDown;
        internal short repeatCount;
        internal short virtualKeyCode;
        internal short virtualScanCode;
        internal ushort uChar; // Union between WCHAR and ASCII char
        internal int controlKeyState;
    }

    // Really, this is a union of KeyEventRecords and other types.
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct InputRecord
    {
        internal short eventType;
        internal KeyEventRecord keyEvent;
        // This struct is a union!  Word alignment should take care of padding!
    }

    internal static partial class Kernel32
    {
        //TODO Add GeneratedDllImport
        //[GeneratedDllImport(Libraries.Kernel32, EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode, SetLastError = true)]
        [DllImport(Libraries.Kernel32, EntryPoint = "ReadConsoleInputW")]
        //internal static partial bool ReadConsoleInput(IntPtr hConsoleInput, out InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);
        internal static extern bool ReadConsoleInput(IntPtr hConsoleInput, out InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);
    }
}
