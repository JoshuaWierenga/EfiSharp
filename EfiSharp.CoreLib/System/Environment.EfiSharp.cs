// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System
{
    //TODO Add more from this file and add the main Environment file
    public static partial class Environment
    {
        internal const string NewLineConst = "\r\n";

        //TODO Move to main Environment file
        public static string NewLine => NewLineConst;
    }
}
