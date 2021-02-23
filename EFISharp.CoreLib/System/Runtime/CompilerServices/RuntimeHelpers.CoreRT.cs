// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [StructLayout(LayoutKind.Sequential)]
    internal class RawData
    {
        public byte Data;
    }
}
