// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

internal static partial class Interop
{
    //TODO Use CoreLib version
    internal static partial class User32
    {
        [DllImport(Libraries.User32)]
        internal static extern short GetKeyState(int virtualKeyCode);
    }
}
