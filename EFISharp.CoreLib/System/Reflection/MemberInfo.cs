﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.CompilerServices;

namespace System.Reflection
{
    public abstract partial class MemberInfo : ICustomAttributeProvider
    {
        public abstract MemberTypes MemberType { get; }
        public abstract string Name { get; }
        //TODO Add Nullable
        //public abstract Type? DeclaringType { get; }
        public abstract Type DeclaringType { get; }
        //TODO Add Nullable
        //public abstract Type? ReflectedType { get; }
        public abstract Type ReflectedType { get; }

        //TODO Add Module
        /*public virtual Module Module
        {
            get
            {
                // This check is necessary because for some reason, Type adds a new "Module" property that hides the inherited one instead
                // of overriding.

                if (this is Type type)
                    return type.Module;

                throw NotImplemented.ByDesign;
            }
        }*/

        //TODO Add NotImplemented
        //public virtual bool HasSameMetadataDefinitionAs(MemberInfo other) { throw NotImplemented.ByDesign; }

        public abstract bool IsDefined(Type attributeType, bool inherit);
        public abstract object[] GetCustomAttributes(bool inherit);
        public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

        //TODO Add IEnumerable<T>, IList<T>, CustomAttributeData and NotImplemented
        //public virtual IEnumerable<CustomAttributeData> CustomAttributes => GetCustomAttributesData();
        //public virtual IList<CustomAttributeData> GetCustomAttributesData() { throw NotImplemented.ByDesign; }
        public virtual bool IsCollectible => true;
        //TODO Fix throwing
        //public virtual int MetadataToken => throw new InvalidOperationException();

        public override bool Equals(object? obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //TODO Add Nullable
        //public static bool operator ==(MemberInfo? left, MemberInfo? right)
        public static bool operator ==(MemberInfo left, MemberInfo right)
        {
            // Test "right" first to allow branch elimination when inlined for null checks (== null)
            // so it can become a simple test
            if (right is null)
            {
                // return true/false not the test result https://github.com/dotnet/runtime/issues/4207
                return (left is null) ? true : false;
            }

            // Try fast reference equality and opposite null check prior to calling the slower virtual Equals
            if ((object?)left == (object)right)
            {
                return true;
            }

            return (left is null) ? false : left.Equals(right);
        }

        //TODO Add Nullable
        //public static bool operator !=(MemberInfo? left, MemberInfo? right) => !(left == right);
        public static bool operator !=(MemberInfo left, MemberInfo right) => !(left == right);
    }
}
