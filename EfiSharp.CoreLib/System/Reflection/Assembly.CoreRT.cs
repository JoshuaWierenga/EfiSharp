﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System.Reflection
{
    //TODO Add ISerializable
    public abstract partial class Assembly : ICustomAttributeProvider//, ISerializable
    {
        //TODO Add StartupCodeHelpers.GetEntryAssembly
        //private static Assembly? GetEntryAssemblyInternal() => Internal.Runtime.CompilerHelpers.StartupCodeHelpers.GetEntryAssembly();

        [System.Runtime.CompilerServices.Intrinsic]
        public static Assembly GetExecutingAssembly() { throw NotImplemented.ByDesign; } //Implemented by toolchain.

        //TODO Add AppContext and GetEntryAssembly
        /*public static Assembly GetCallingAssembly()
        {
            if (AppContext.TryGetSwitch("Switch.System.Reflection.Assembly.SimulatedCallingAssembly", out bool isSimulated) && isSimulated)
                return GetEntryAssembly();

            throw new PlatformNotSupportedException();
        }*/

        //TODO Add ReflectionAugments
        //public static Assembly Load(AssemblyName assemblyRef) => ReflectionAugments.ReflectionCoreCallbacks.Load(assemblyRef, throwOnFileNotFound: true);

        //TODO Add AssemblyName(String) and Load(AssemblyName)
        /*public static Assembly Load(string assemblyString)
        {
            if (assemblyString == null)
                throw new ArgumentNullException(nameof(assemblyString));

            AssemblyName name = new AssemblyName(assemblyString);
            return Load(name);
        }*/

        //TODO Add Load(AssemblyName) and FileNotFoundException
        /*[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. https://go.microsoft.com/fwlink/?linkid=14202")]
        public static Assembly LoadWithPartialName(string partialName)
        {
            if (partialName == null)
                throw new ArgumentNullException(nameof(partialName));

            if ((partialName.Length == 0) || (partialName[0] == '\0'))
                throw new ArgumentException(SR.Format_StringZeroLength, nameof(partialName));

            try
            {
                return Load(partialName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }*/
    }
}
