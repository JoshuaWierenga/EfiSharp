﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System.Reflection
{
    //TODO Support ICustomAttributeProvider
    //TODO Add IObjectReference
    public class ParameterInfo //: ICustomAttributeProvider, IObjectReference
    {
        protected ParameterInfo() { }

        public virtual ParameterAttributes Attributes => AttrsImpl;
        public virtual MemberInfo Member => MemberImpl;
        public virtual string? Name => NameImpl;
        public virtual Type ParameterType => ClassImpl!;
        public virtual int Position => PositionImpl;

        public bool IsIn => (Attributes & ParameterAttributes.In) != 0;
        public bool IsLcid => (Attributes & ParameterAttributes.Lcid) != 0;
        public bool IsOptional => (Attributes & ParameterAttributes.Optional) != 0;
        public bool IsOut => (Attributes & ParameterAttributes.Out) != 0;
        public bool IsRetval => (Attributes & ParameterAttributes.Retval) != 0;

        public virtual object? DefaultValue => throw NotImplemented.ByDesign;
        public virtual object? RawDefaultValue => throw NotImplemented.ByDesign;
        public virtual bool HasDefaultValue => throw NotImplemented.ByDesign;

        public virtual bool IsDefined(Type attributeType, bool inherit)
        {
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            return false;
        }

        //TODO Add IEnumerable, IList and NotImplemented
        /*public virtual IEnumerable<CustomAttributeData> CustomAttributes => GetCustomAttributesData();
        public virtual IList<CustomAttributeData> GetCustomAttributesData() { throw NotImplemented.ByDesign; }*/

        //TODO Add Array.Empty<T>
        /*public virtual object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();
        public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            return Array.Empty<object>();
        }*/


        //TODO Add Type.EmptyTypes
        /*public virtual Type[] GetOptionalCustomModifiers() => Type.EmptyTypes;
        public virtual Type[] GetRequiredCustomModifiers() => Type.EmptyTypes;*/

        public virtual int MetadataToken => MetadataToken_ParamDef;

        //TODO Add StreamingContext, SerializationException(String) and PropertyInfo
        /*public object GetRealObject(StreamingContext context)
        {
            // Once all the serializable fields have come in we can set up the real
            // instance based on just two of them (MemberImpl and PositionImpl).

            if (MemberImpl == null)
                throw new SerializationException(SR.Serialization_InsufficientState);

            ParameterInfo[] args;
            switch (MemberImpl.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                    if (PositionImpl == -1)
                    {
                        if (MemberImpl.MemberType == MemberTypes.Method)
                            return ((MethodInfo)MemberImpl).ReturnParameter;
                        else
                            throw new SerializationException(SR.Serialization_BadParameterInfo);
                    }
                    else
                    {
                        args = ((MethodBase)MemberImpl).GetParametersNoCopy();

                        if (args != null && PositionImpl < args.Length)
                            return args[PositionImpl];
                        else
                            throw new SerializationException(SR.Serialization_BadParameterInfo);
                    }

                case MemberTypes.Property:
                    args = ((PropertyInfo)MemberImpl).GetIndexParameters();

                    if (args != null && PositionImpl > -1 && PositionImpl < args.Length)
                        return args[PositionImpl];
                    else
                        throw new SerializationException(SR.Serialization_BadParameterInfo);

                default:
                    throw new SerializationException(SR.Serialization_NoParameterInfo);
            }
        }*/

        //TODO Add Object.ToString and ParameterType.FormatTypeName
        //public override string ToString() => ParameterType.FormatTypeName() + " " + Name;

        protected ParameterAttributes AttrsImpl;
        //TODO Add Nullable
        //protected Type? ClassImpl;
        protected Type ClassImpl;
        //TODO Add Nullable
        //protected object? DefaultValueImpl;
        protected object DefaultValueImpl;
        protected MemberInfo MemberImpl = null!;
        //TODO Add Nullable
        //protected string? NameImpl;
        protected string NameImpl;
        protected int PositionImpl;

        private const int MetadataToken_ParamDef = 0x08000000;

    }
}