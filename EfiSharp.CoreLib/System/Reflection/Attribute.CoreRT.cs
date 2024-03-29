﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace System
{

    //TODO This attribute should be on Attribute.cs but the version I imported from Runtime.Base wasn't partial, get System.Private.CoreLib version of that file
    //doesn't matter since currently this file isn't supported
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public abstract partial class Attribute
    {
        //TODO Add Assembly.GetMatchingCustomAttributes
        /*public static Attribute GetCustomAttribute(Assembly element, Type attributeType)
        {
            return OneOrNull(element.GetMatchingCustomAttributes(attributeType));
        }
        public static Attribute GetCustomAttribute(Assembly element, Type attributeType, bool inherit) => GetCustomAttribute(element, attributeType); // "inherit" is meaningless for assemblies

        public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType) => GetCustomAttribute(element, attributeType, inherit: true);
        public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType, bool inherit)
        {
            return OneOrNull(element.GetMatchingCustomAttributes(attributeType, inherit));
        }

        public static Attribute GetCustomAttribute(Module element, Type attributeType)
        {
            return OneOrNull(element.GetMatchingCustomAttributes(attributeType));
        }
        public static Attribute GetCustomAttribute(Module element, Type attributeType, bool inherit) => GetCustomAttribute(element, attributeType);*/ // "inherit" is meaningless for modules

        //TODO Add CustomAttributeExtensions.GetCustomAttribute and ParameterInfo.GetMatchingCustomAttributes
        /*public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType) => CustomAttributeExtensions.GetCustomAttribute(element, attributeType, inherit: true);
        public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, bool inherit)
        {
            return OneOrNull(element.GetMatchingCustomAttributes(attributeType, inherit));
        }*/

        //TODO Add IEnumerable<T>, CustomAttributeData and Assembly.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(Assembly element)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(null, skipTypeValidation: true);
            return matches.Select(m => m.Instantiate()).ToArray();
        }
        public static Attribute[] GetCustomAttributes(Assembly element, bool inherit) => GetCustomAttributes(element);*/ // "inherit" is meaningless for assemblies
        //TODO Add Assembly.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType)
        {
            return Instantiate(element.GetMatchingCustomAttributes(attributeType), attributeType);
        }
        public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType, bool inherit) => GetCustomAttributes(element, attributeType);*/ // "inherit" is meaningless for modules

        //TODO Add IEnumerable<T>, CustomAttributeData and MemberInfo.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(MemberInfo element) => GetCustomAttributes(element, inherit: true);
        public static Attribute[] GetCustomAttributes(MemberInfo element, bool inherit)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(null, inherit, skipTypeValidation: true);
            return matches.Select(m => m.Instantiate()).ToArray();
        }*/
        //TODO Add MemberInfo.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(MemberInfo element, Type attributeType) => GetCustomAttributes(element, attributeType, inherit: true);
        public static Attribute[] GetCustomAttributes(MemberInfo element, Type attributeType, bool inherit)
        {
            return Instantiate(element.GetMatchingCustomAttributes(attributeType, inherit), attributeType);
        }*/

        //TODO Add IEnumerable<T>, CustomAttributeData and Module.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(Module element)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(null, skipTypeValidation: true);
            return matches.Select(m => m.Instantiate()).ToArray();
        }
        public static Attribute[] GetCustomAttributes(Module element, bool inherit) => GetCustomAttributes(element);*/ // "inherit" is meaningless for assemblies
        //TODO Add Module.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(Module element, Type attributeType)
        {
            return Instantiate(element.GetMatchingCustomAttributes(attributeType), attributeType);
        }
        public static Attribute[] GetCustomAttributes(Module element, Type attributeType, bool inherit) => GetCustomAttributes(element, attributeType);*/ // "inherit" is meaningless for modules

        //TODO Add IEnumerable<T>, CustomAttributeData and ParameterInfo.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(ParameterInfo element) => GetCustomAttributes(element, inherit: true);
        public static Attribute[] GetCustomAttributes(ParameterInfo element, bool inherit)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(null, inherit, skipTypeValidation: true);
            return matches.Select(m => m.Instantiate()).ToArray();
        }*/
        //TODO Add ParameterInfo.GetMatchingCustomAttributes
        /*public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType) => GetCustomAttributes(element, attributeType, inherit: true);
        public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, bool inherit)
        {
            return Instantiate(element.GetMatchingCustomAttributes(attributeType, inherit), attributeType);
        }*/

        //TODO Add IEnumerable<T>, CustomAttributeData and Assembly.GetMatchingCustomAttributes
        /*public static bool IsDefined(Assembly element, Type attributeType)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(attributeType);
            return matches.Any();
        }
        public static bool IsDefined(Assembly element, Type attributeType, bool inherit) => IsDefined(element, attributeType); // "inherit" is meaningless for assemblies

        public static bool IsDefined(MemberInfo element, Type attributeType) => IsDefined(element, attributeType, inherit: true);
        public static bool IsDefined(MemberInfo element, Type attributeType, bool inherit)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(attributeType, inherit);
            return matches.Any();
        }*/

        //TODO Add IEnumerable<T>, CustomAttributeData and Module.GetMatchingCustomAttributes
        /*public static bool IsDefined(Module element, Type attributeType)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(attributeType);
            return matches.Any();
        }
        public static bool IsDefined(Module element, Type attributeType, bool inherit) => IsDefined(element, attributeType);*/ // "inherit" is meaningless for modules

        //TODO Add IEnumerable<T>, CustomAttributeData and ParameterInfo.GetMatchingCustomAttributes
        /*public static bool IsDefined(ParameterInfo element, Type attributeType) => IsDefined(element, attributeType, inherit: true);
        public static bool IsDefined(ParameterInfo element, Type attributeType, bool inherit)
        {
            IEnumerable<CustomAttributeData> matches = element.GetMatchingCustomAttributes(attributeType, inherit);
            return matches.Any();
        }*/

        //==============================================================================================================================
        // Helper for the GetCustomAttribute() family.
        //==============================================================================================================================
        //TODO Add IEnumerable<T>, CustomAttributeData, IEnumerator<T> and AmbiguousMatchException
        /*private static Attribute OneOrNull(IEnumerable<CustomAttributeData> results)
        {
            IEnumerator<CustomAttributeData> enumerator = results.GetEnumerator();
            if (!enumerator.MoveNext())
                return null;
            CustomAttributeData result = enumerator.Current;
            if (enumerator.MoveNext())
                throw new AmbiguousMatchException();
            return result.Instantiate();
        }*/

        //==============================================================================================================================
        // Helper for the GetCustomAttributes() methods that take a specific attribute type. For desktop compatibility,
        // we return a freshly allocated array of the specific attribute type even though the api's return type promises only an Attribute[].
        // There are known store apps that cast the results of apis and expect the cast to work.
        //==============================================================================================================================
        //TODO Add IEnumerable<T>, CustomAttributeData, LowLevelList and Array.CreateInstance
        /*[UnconditionalSuppressMessage("AotAnalysis", "IL9700:RequiresDynamicCode",
            Justification = "Arrays of reference types are safe to create.")]
        private static Attribute[] Instantiate(IEnumerable<CustomAttributeData> cads, Type actualElementType)
        {
            LowLevelList<Attribute> attributes = new LowLevelList<Attribute>();
            foreach (CustomAttributeData cad in cads)
            {
                Attribute instantiatedAttribute = cad.Instantiate();
                attributes.Add(instantiatedAttribute);
            }
            int count = attributes.Count;
            Attribute[] result;
            try
            {
                result = (Attribute[])Array.CreateInstance(actualElementType, count);
            }
            catch (NotSupportedException) when (actualElementType.ContainsGenericParameters)
            {
                // This is here for desktop compatibility (using try-catch as control flow to avoid slowing down the mainline case.)
                // GetCustomAttributes() normally returns an array of the exact attribute type requested except when
                // the requested type is an open type. Its ICustomAttributeProvider counterpart would return an Object[] array but that's
                // not possible with this api's return type so it returns null instead.
                return null;
            }
            attributes.CopyTo(result, 0);
            return result;
        }*/
    }
}