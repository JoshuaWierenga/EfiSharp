﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.InteropServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System
{
    // CONTRACT with Runtime
    // The Object type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type EEType*
    // VTable Contract: The first vtable slot should be the finalizer for object => The first virtual method in the object class should be the Finalizer
    public unsafe class Object
    {
        internal EEType* m_pEEType;

        // Creates a new instance of an Object.
        public Object()
        {
        }

        // Allow an object to free resources before the object is reclaimed by the GC.
        // CONTRACT with runtime: This method's virtual slot number is hardcoded in the binder. It is an
        // implementation detail where it winds up at runtime.
        // **** Do not add any virtual methods in this class ahead of this ****
        ~Object()
        {
        }

        //From https://github.com/Michael-Kelley/RoseOS/blob/ecd805014a/CoreLib/System/Object.cs#L21
        public void Dispose()
        {
            object obj = this;
            EfiSharp.UefiApplication.SystemTable->BootServices->FreePool((void*)Unsafe.As<object, IntPtr>(ref obj));
        }

        public virtual bool Equals(object o)
        {
            return false;
        }

        public virtual int GetHashCode()
        {
            return 0;
        }

        internal EEType* EEType
        {
            get
            {
                // NOTE:  if managed code can be run when the GC has objects marked, then this method is
                //        unsafe.  But, generically, we don't expect managed code such as this to be allowed
                //        to run while the GC is running.
                return m_pEEType;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private class RawData
        {
            public byte Data;
        }

        internal ref byte GetRawData()
        {
            return ref Unsafe.As<RawData>(this).Data;
        }

        /// <summary>
        /// Return size of all data (excluding ObjHeader and EEType*).
        /// Note that for strings/arrays this would include the Length as well.
        /// </summary>
        internal uint GetRawDataSize()
        {
            return EEType->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(EEType*);
        }
    }
}