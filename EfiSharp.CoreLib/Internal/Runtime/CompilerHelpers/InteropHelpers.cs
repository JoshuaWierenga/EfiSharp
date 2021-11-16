// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

namespace Internal.Runtime.CompilerHelpers
{
    /// <summary>
    /// These methods are used to throw exceptions from generated code.
    /// </summary>
    internal static class InteropHelpers
    {
        internal static unsafe byte* StringToAnsiString(string str, bool bestFit, bool throwOnUnmappableChar)
        {
            // This is definitely a weird return but removing the return type gives a compiler warning so this is the best option
            // short of actually implementing this method which appears redundant since even the void return version works fine
            return (byte*)0;
        }

        internal static unsafe void CoTaskMemFree(void* p) { }
    }
}
