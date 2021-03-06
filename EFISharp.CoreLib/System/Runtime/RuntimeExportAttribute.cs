// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System.Runtime
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class RuntimeExportAttribute : Attribute
    {
        public string EntryPoint;

        public RuntimeExportAttribute(string entry)
        {
            EntryPoint = entry;
        }
    }
}
