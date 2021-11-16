// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

namespace System.IO
{
    // Contains constants for specifying how the OS should open a file.
    // These will control whether you overwrite a file, open an existing
    // file, or some combination thereof.
    //
    // To append to a file, use Append (which maps to OpenOrCreate then we seek
    // to the end of the file).  To truncate a file or create it if it doesn't
    // exist, use Create.
    //
    //TODO Use CoreLib version
    public enum FileMode
    {
        // Opens an existing file. An exception is raised if the file does not exist.
        Open = 3,
    }
}