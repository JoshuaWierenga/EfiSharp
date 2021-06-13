// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System
{
    public static partial class AppContext
    {
        public static void SetData(string name, object? data)
        {
            //TODO Actually support, is this required any time soon though since
            //this was only added to appease the compiler. 
            throw new NotImplementedException(SR.Arg_NotImplementedException);
        }
    }
}