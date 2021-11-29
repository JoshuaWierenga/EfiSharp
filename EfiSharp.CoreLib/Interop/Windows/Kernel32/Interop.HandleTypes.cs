// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        //TODO Use CoreLib version
        internal static partial class HandleTypes
        {
            internal const int STD_INPUT_HANDLE = -10;
            internal const int STD_OUTPUT_HANDLE = -11;
        }
    }
}
