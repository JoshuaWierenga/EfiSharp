// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using Internal.Runtime.CompilerServices;

namespace System
{
    //TODO Add main Environment file
    public static partial class Environment
    {
        internal const string NewLineConst = "\r\n";

        //TODO Move to main Environment file
        public static string NewLine => NewLineConst;

        //TODO Fix issues with static reference fields and then store string[] directly
        private static IntPtr s_commandLineArgs;

        internal static void SetCommandLineArgs(string[] cmdLineArgs) // invoked from VM
        {
            s_commandLineArgs = Unsafe.As<string[], IntPtr>(ref cmdLineArgs);
        }

        public static string[] GetCommandLineArgs() => Unsafe.As<IntPtr, string[]>(ref s_commandLineArgs);
    }
}
