// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        //TODO Add GeneratedDllImportAttribute
        //[GeneratedDllImport(Libraries.Kernel32, EntryPoint = "FillConsoleOutputCharacterW", CharSet = CharSet.Unicode, SetLastError = true)]
        [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, EntryPoint = "FillConsoleOutputCharacterW")]
        //internal static partial bool FillConsoleOutputCharacter(IntPtr hConsoleOutput, char character, int nLength, COORD dwWriteCoord, out int pNumCharsWritten);
        internal static extern bool FillConsoleOutputCharacter(IntPtr hConsoleOutput, char character, int nLength, COORD dwWriteCoord, out int pNumCharsWritten);
    }
}
