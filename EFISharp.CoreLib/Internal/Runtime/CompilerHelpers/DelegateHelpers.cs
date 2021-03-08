// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System;

namespace Internal.Runtime.CompilerHelpers
{
    /// <summary>
    /// Delegate helpers for generated code.
    /// </summary>
    internal static class DelegateHelpers
    {
        //TODO Add Array.Empty and fix static array issues
        //private static object[] s_emptyObjectArray = Array.Empty<object>();

        internal static object[] GetEmptyObjectArray()
        {
            //return s_emptyObjectArray;
            return new object[0];
        }
    }
}