﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

/*=============================================================================
**
**
**
** Purpose: Exception class for invalid array indices.
**
**
=============================================================================*/

namespace System
{
    [Serializable]
    [System.Runtime.CompilerServices.TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    public sealed class IndexOutOfRangeException : SystemException
    {
        public IndexOutOfRangeException()
            : base(SR.Arg_IndexOutOfRangeException)
        {
            HResult = HResults.COR_E_INDEXOUTOFRANGE;
        }

        public IndexOutOfRangeException(string? message)
            : base(message)
        {
            HResult = HResults.COR_E_INDEXOUTOFRANGE;
        }

        public IndexOutOfRangeException(string? message, Exception? innerException)
            : base(message, innerException)
        {
            HResult = HResults.COR_E_INDEXOUTOFRANGE;
        }

        /*private IndexOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }*/
    }
}