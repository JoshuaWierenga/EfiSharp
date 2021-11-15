// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

internal static partial class Interop
{
    internal static partial class BCrypt
    {
        //TODO Use CoreLib version
        internal enum NTSTATUS : uint
        {
            STATUS_SUCCESS = 0x0,
            STATUS_NO_MEMORY = 0xc0000017,
        }
    }
}