// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.Serialization;

namespace System.Reflection
{
    //TODO Support ICloneable
    //TODO Add ISerializable
    public sealed partial class AssemblyName :/* ICloneable,*/ IDeserializationCallback//, ISerializable
    {
        //TODO Add RuntimeAssemblyName and AssemblyNameParser
        /*public AssemblyName(string assemblyName)
            : this()
        {
            if (assemblyName == null)
                throw new ArgumentNullException(nameof(assemblyName));
            if ((assemblyName.Length == 0) ||
                (assemblyName[0] == '\0'))
                throw new ArgumentException(SR.Format_StringZeroLength);

            _name = assemblyName;
            RuntimeAssemblyName runtimeAssemblyName = AssemblyNameParser.Parse(_name);
            runtimeAssemblyName.CopyToAssemblyName(this);
        }*/

        //TODO Add AssemblyNameHelpers
        /*private byte[] ComputePublicKeyToken()
        {
            return AssemblyNameHelpers.ComputePublicKeyToken(_publicKey);
        }*/

        public static AssemblyName GetFileInformationCore(string assemblyFile)
        {
            throw new PlatformNotSupportedException(SR.Arg_PlatformNotSupported_AssemblyName_GetAssemblyName);
        }
    }
}