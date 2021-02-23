﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.CompilerServices;

using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System
{
    // CONTRACT with Runtime
    // The Object type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type EEType_ptr (or void * till a tool bug can be fixed)
    // VTable Contract: The first vtable slot should be the finalizer for object => The first virtual method in the object class should be the Finalizer

    public unsafe partial class Object
    {
        // Marked as internal for now so that some classes (System.Buffer, System.Enum) can use C#'s fixed
        // statement on partially typed objects. Wouldn't have to do this if we could directly declared pinned
        // locals.
        //[NonSerialized]
        internal EEType* m_pEEType;

#if INPLACE_RUNTIME
        internal unsafe EEType* EEType
        {
            get
            {
                return m_pEEType;
            }
        }
#endif

        //TODO Add Type.GetTypeFromEETypePtr
        /*[Intrinsic]
        public Type GetType()
        {
            return Type.GetTypeFromEETypePtr(EETypePtr);
        }*/

        internal EETypePtr EETypePtr
        {
            get
            {
                //TODO Add EEType constructor for EETypePtr
                //return new EETypePtr(m_pEEType);
                return new EETypePtr((IntPtr)(void*)m_pEEType);
            }
        }

        //TODO Add RuntimeImports.RhMemberwiseClone
        /*[Intrinsic]
        protected object MemberwiseClone()
        {
            return RuntimeImports.RhMemberwiseClone(this);
        }*/

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