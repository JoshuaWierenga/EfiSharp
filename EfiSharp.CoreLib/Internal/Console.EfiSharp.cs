// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using EfiSharp;

namespace Internal
{
    public static partial class Console
    {
        public static unsafe void Write(string s)
        {
            UefiApplication.Out->OutputString(s);
        }

        public static partial class Error
        {
            public static unsafe void Write(string s)
            {
                //TODO Add StdErr support
                nuint currentColours = (nuint)UefiApplication.Out->Mode->Attribute;
                //Set foreground colour to red and background colour to black
                UefiApplication.Out->SetAttribute(12);
                UefiApplication.Out->OutputString(s);
                UefiApplication.Out->SetAttribute(currentColours);
            }
        }
    }
}
