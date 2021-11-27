// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

using System;

namespace Internal.Runtime.CompilerHelpers
{
    public partial class StartupCodeHelpers
    {
        internal static unsafe void InitializeCommandLineArgsW(int argc, char** argv)
        {
            Environment.SetCommandLineArgs(new string[0]);
        }

        private static string[] GetMainMethodArguments()
        {
            return Environment.GetCommandLineArgs();
        }

        private static void SetLatchedExitCode(int exitCode)
        {
            Environment.ExitCode = exitCode;
        }

        // Shuts down the class library and returns the process exit code.
        private static int Shutdown()
        {
            Environment.GetCommandLineArgs().Free();

            return Environment.ExitCode;
        }
    }
}
