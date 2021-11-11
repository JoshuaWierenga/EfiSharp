// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace System
{
    // Base class for runtime implemented Type
    //TODO Add TypeInfo and then use correct definition
    //public abstract class RuntimeType : TypeInfo
    public abstract class RuntimeType : Type
    {
        //TODO Add Type{Info?}.GetEnumName, Enum.TryGetUnboxedValueOfEnumOrInteger, Type.IsEnum and Enum.GetEnumName
        /*public sealed override string GetEnumName(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            ulong rawValue;
            if (!Enum.TryGetUnboxedValueOfEnumOrInteger(value, out rawValue))
                throw new ArgumentException(SR.Arg_MustBeEnumBaseTypeOrEnum, nameof(value));

            // For desktop compatibility, do not bounce an incoming integer that's the wrong size.
            // Do a value-preserving cast of both it and the enum values and do a 64-bit compare.

            if (!IsEnum)
                throw new ArgumentException(SR.Arg_MustBeEnum);

            return Enum.GetEnumName(this, rawValue);
        }*/

        //TODO Add Type{Info?}.GetEnumNames, Type{Info?}.IsEnum, Enum.InternalGetNames and ReadOnlySpan<T>
        /*public sealed override string[] GetEnumNames()
        {
            if (!IsEnum)
                throw new ArgumentException(SR.Arg_MustBeEnum, "enumType");

            string[] ret = Enum.InternalGetNames(this);

            // Make a copy since we can't hand out the same array since users can modify them
            return new ReadOnlySpan<string>(ret).ToArray();
        }*/

        //TODO Add Type{Info?}.GetEnumUnderlyingType, Type{Info?}.IsEnum and Enum.InternalGetUnderlyingType
        /*public sealed override Type GetEnumUnderlyingType()
        {
            if (!IsEnum)
                throw new ArgumentException(SR.Arg_MustBeEnum, "enumType");

            return Enum.InternalGetUnderlyingType(this);
        }*/

        //TODO Add Type{Info?}.IsEnumDefined, Type{Info?}.IsEnum, EnumInfo, Enum.GetEnumInfo, Enum.TryGetUnboxedValueOfEnumOrInteger, Type.IsIntegerType
        //TODO Add  Object.GetType, SR.Format, Enum.InternalGetUnderlyingType, Enum.ValueTypeMatchesEnumType and Enum.GetEnumName
        /*public sealed override bool IsEnumDefined(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!IsEnum)
                throw new ArgumentException(SR.Arg_MustBeEnum);

            if (value is string valueAsString)
            {
                EnumInfo enumInfo = Enum.GetEnumInfo(this);
                foreach (string name in enumInfo.Names)
                {
                    if (valueAsString == name)
                        return true;
                }
                return false;
            }
            else
            {
                ulong rawValue;
                if (!Enum.TryGetUnboxedValueOfEnumOrInteger(value, out rawValue))
                {
                    if (Type.IsIntegerType(value.GetType()))
                        throw new ArgumentException(SR.Format(SR.Arg_EnumUnderlyingTypeAndObjectMustBeSameType, value.GetType(), Enum.InternalGetUnderlyingType(this)));
                    else
                        throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
                }

                if (value is Enum)
                {
                    if (!Enum.ValueTypeMatchesEnumType(this, value))
                        throw new ArgumentException(SR.Format(SR.Arg_EnumAndObjectMustBeSameType, value.GetType(), this));
                }
                else
                {
                    Type underlyingType = Enum.InternalGetUnderlyingType(this);
                    if (!(underlyingType.TypeHandle.ToEETypePtr() == value.EETypePtr))
                        throw new ArgumentException(SR.Format(SR.Arg_EnumUnderlyingTypeAndObjectMustBeSameType, value.GetType(), underlyingType));
                }

                return Enum.GetEnumName(this, rawValue) != null;
            }
        }*/

        //TODO Add Type{Info?}.GetEnumValues, Type{Info?}.IsEnum, Enum.GetEnumName, AppContext.TryGetSwitch, Array.CreateInstance, Enum.InternalGetUnderlyingType
        //TODO Add Array.Copy
        /*[RequiresDynamicCode("It might not be possible to create an array of the enum type at runtime. Use the GetValues<TEnum> overload instead.")]
        public sealed override Array GetEnumValues()
        {
            if (!IsEnum)
                throw new ArgumentException(SR.Arg_MustBeEnum, "enumType");

            Array values = Enum.GetEnumInfo(this).ValuesAsUnderlyingType;
            int count = values.Length;
            // Without universal shared generics, chances are slim that we'll have the appropriate
            // array type available. Offer an escape hatch that avoids a MissingMetadataException
            // at the cost of a small appcompat risk.
            Array result;
            if (AppContext.TryGetSwitch("Switch.System.Enum.RelaxedGetValues", out bool isRelaxed) && isRelaxed)
                result = Array.CreateInstance(Enum.InternalGetUnderlyingType(this), count);
            else
                result = Array.CreateInstance(this, count);
            Array.Copy(values, result, values.Length);
            return result;
        }*/
    }
}