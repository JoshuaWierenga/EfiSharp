﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System;

namespace Internal.Runtime.CompilerHelpers
{
    /// <summary>
    /// These methods are used to implement TypedReference-related instructions.
    /// </summary>
    internal static class TypedReferenceHelpers
    {
        //TODO Add Type.GetTypeFromHandle
        /*public static Type TypeHandleToRuntimeTypeMaybeNull(RuntimeTypeHandle typeHandle)
        {
            if (typeHandle.IsNull)
                return null;
            return Type.GetTypeFromHandle(typeHandle);
        }*/

        public static RuntimeTypeHandle TypeHandleToRuntimeTypeHandleMaybeNull(RuntimeTypeHandle typeHandle)
        {
            return typeHandle;
        }

        public static ref byte GetRefAny(RuntimeTypeHandle type, TypedReference typedRef)
        {
            if (!TypedReference.RawTargetTypeToken(typedRef).Equals(type))
            {
                throw new InvalidCastException();
            }

            return ref typedRef.Value;
        }
    }
}