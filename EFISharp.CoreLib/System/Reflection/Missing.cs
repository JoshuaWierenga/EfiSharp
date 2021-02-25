// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System.Reflection
{
    //TODO Add ISerializable
    public class Missing //: ISerializable
    {
        public static readonly Missing Value = new Missing();

        private Missing() { }

        //TODO Add SerializationInfo and StreamingContext
        /*void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new PlatformNotSupportedException();
        }*/
    }
}