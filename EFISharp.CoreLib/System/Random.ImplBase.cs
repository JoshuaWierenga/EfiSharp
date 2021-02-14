// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System
{
    public partial class Random
    {
        /// <summary>Base type for all generator implementations that plug into the base Random.</summary>
        internal abstract class ImplBase
        {
            public abstract void NextBytes(byte[] buffer);
        }
    }
}
