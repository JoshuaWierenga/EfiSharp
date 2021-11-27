// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

namespace System
{
    public static partial class Environment
    {
        private static int s_latchedExitCode;

        public static int ExitCode
        {
            get => s_latchedExitCode;
            set => s_latchedExitCode = value;
        }

    }
}
