// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System;

namespace Internal.Runtime.CompilerServices
{
    [System.Runtime.CompilerServices.ReflectionBlocked]
    public struct GenericMethodDescriptor
    {
        public readonly IntPtr MethodFunctionPointer;
        public readonly IntPtr InstantiationArgument;

        //TODO Add ValueTuple`2'
        /*public GenericMethodDescriptor(IntPtr methodFunctionPointer, IntPtr instantiationArgument)
            => (MethodFunctionPointer, InstantiationArgument) = (methodFunctionPointer, instantiationArgument);*/
    }
}
