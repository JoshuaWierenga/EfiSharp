﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System
{
    //TODO Add IComparable, IConvertible, ISpanFormattable and IComparable<T>
    [Serializable]
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
    [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    public readonly struct UInt32 : /*IComparable, IConvertible, ISpanFormattable, IComparable<uint>,*/ IEquatable<uint>
#if FEATURE_GENERIC_MATH //TODO Fix interfaces, are generics broken as well?
#pragma warning disable SA1001, CA2252 // SA1001: Comma positioning; CA2252: Preview Features
        , IBinaryInteger<uint>,
          IMinMaxValue<uint>,
          IUnsignedNumber<uint>
#pragma warning restore SA1001, CA2252
#endif // FEATURE_GENERIC_MATH
    {
        private readonly uint m_value; // Do not rename (binary serialization)

        public const uint MaxValue = (uint)0xffffffff;
        public const uint MinValue = 0U;

        // Compares this object to another object, returning an integer that
        // indicates the relationship.
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type UInt32, this method throws an ArgumentException.
        //
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return 1;
            }

            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (value is uint i)
            {
                if (m_value < i) return -1;
                if (m_value > i) return 1;
                return 0;
            }

            throw new ArgumentException(SR.Arg_MustBeUInt32);
        }

        public int CompareTo(uint value)
        {
            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            return 0;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is uint))
            {
                return false;
            }
            return m_value == ((uint)obj).m_value;
        }

        [NonVersionable]
        public bool Equals(uint obj)
        {
            return m_value == obj;
        }

        // The absolute value of the int contained.
        public override int GetHashCode()
        {
            return (int)m_value;
        }

        // The base 10 representation of the number with no extra padding.
        public override string ToString()
        {
            return Number.UInt32ToDecStr(m_value);
        }

        public string ToString(IFormatProvider? provider)
        {
            return Number.UInt32ToDecStr(m_value);
        }

        //TODO Add Number.FormatUInt32
        /*public string ToString(string? format)
        {
            return Number.FormatUInt32(m_value, format, null);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            return Number.FormatUInt32(m_value, format, provider);
        }*/

        //TODO Add Span<T>, ReadOnlySpan<T> and Number.TryFormatUInt32
        /*public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            return Number.TryFormatUInt32(m_value, format, provider, destination, out charsWritten);
        }*/

        //TODO Add Number.ParseUInt32, NumberStyles and NumberFormatInfo
        /*public static uint Parse(string s)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static uint Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt32(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static uint Parse(string s, IFormatProvider? provider)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static uint Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number
        /*public static uint Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add Number, NumberStyles and NumberFormatInfo
        /*public static bool TryParse([NotNullWhen(true)] string? s, out uint result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }

            return Number.TryParseUInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add ReadOnlySpan<T>, Number.TryParseUInt32IntegerStyle, NumberStyles and NumberFormatInfo
        /*public static bool TryParse(ReadOnlySpan<char> s, out uint result)
        {
            return Number.TryParseUInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add NumberStyles, NumberFormatInfo and Number.TryParseUInt32
        /*public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out uint result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);

            if (s == null)
            {
                result = 0;
                return false;
            }

            return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number.TryParseUInt32
        /*public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out uint result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }*/

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt32;
        }

        //TODO Add IConvertible and Convert
        /*bool IConvertible.ToBoolean(IFormatProvider? provider)
        {
            return Convert.ToBoolean(m_value);
        }

        char IConvertible.ToChar(IFormatProvider? provider)
        {
            return Convert.ToChar(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider)
        {
            return Convert.ToSByte(m_value);
        }

        byte IConvertible.ToByte(IFormatProvider? provider)
        {
            return Convert.ToByte(m_value);
        }

        short IConvertible.ToInt16(IFormatProvider? provider)
        {
            return Convert.ToInt16(m_value);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider)
        {
            return Convert.ToUInt16(m_value);
        }

        int IConvertible.ToInt32(IFormatProvider? provider)
        {
            return Convert.ToInt32(m_value);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider)
        {
            return m_value;
        }

        long IConvertible.ToInt64(IFormatProvider? provider)
        {
            return Convert.ToInt64(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider)
        {
            return Convert.ToUInt64(m_value);
        }

        float IConvertible.ToSingle(IFormatProvider? provider)
        {
            return Convert.ToSingle(m_value);
        }

        double IConvertible.ToDouble(IFormatProvider? provider)
        {
            return Convert.ToDouble(m_value);
        }*/

        //TODO Add decimal, IConvertible and Convert
        /*decimal IConvertible.ToDecimal(IFormatProvider? provider)
        {
            return Convert.ToDecimal(m_value);
        }*/

        //TODO Add DateTime, IConvertible and SR.Format
        /*DateTime IConvertible.ToDateTime(IFormatProvider? provider)
        {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "UInt32", "DateTime"));
        }*/

        //TODO Add IConvertible and Convert
        /*object IConvertible.ToType(Type type, IFormatProvider? provider)
        {
            return Convert.DefaultToType((IConvertible)this, type, provider);
        }*/

        //TODO Fix interfaces, are generics broken as well?
#if FEATURE_GENERIC_MATH
        //
        // IAdditionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IAdditionOperators<uint, uint, uint>.operator +(uint left, uint right)
            => left + right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IAdditionOperators<uint, uint, uint>.operator +(uint left, uint right)
        //     => checked(left + right);

        //
        // IAdditiveIdentity
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IAdditiveIdentity<uint, uint>.AdditiveIdentity => 0;

        //
        // IBinaryInteger
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBinaryInteger<uint>.LeadingZeroCount(uint value)
            => (uint)BitOperations.LeadingZeroCount(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBinaryInteger<uint>.PopCount(uint value)
            => (uint)BitOperations.PopCount(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBinaryInteger<uint>.RotateLeft(uint value, int rotateAmount)
            => BitOperations.RotateLeft(value, rotateAmount);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBinaryInteger<uint>.RotateRight(uint value, int rotateAmount)
            => BitOperations.RotateRight(value, rotateAmount);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBinaryInteger<uint>.TrailingZeroCount(uint value)
            => (uint)BitOperations.TrailingZeroCount(value);

        //
        // IBinaryNumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IBinaryNumber<uint>.IsPow2(uint value)
            => BitOperations.IsPow2(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBinaryNumber<uint>.Log2(uint value)
            => (uint)BitOperations.Log2(value);

        //
        // IBitwiseOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBitwiseOperators<uint, uint, uint>.operator &(uint left, uint right)
            => left & right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBitwiseOperators<uint, uint, uint>.operator |(uint left, uint right)
            => left | right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBitwiseOperators<uint, uint, uint>.operator ^(uint left, uint right)
            => left ^ right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IBitwiseOperators<uint, uint, uint>.operator ~(uint value)
            => ~value;

        //
        // IComparisonOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<uint, uint>.operator <(uint left, uint right)
            => left < right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<uint, uint>.operator <=(uint left, uint right)
            => left <= right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<uint, uint>.operator >(uint left, uint right)
            => left > right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<uint, uint>.operator >=(uint left, uint right)
            => left >= right;

        //
        // IDecrementOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IDecrementOperators<uint>.operator --(uint value)
            => --value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IDecrementOperators<uint>.operator --(uint value)
        //     => checked(--value);

        //
        // IDivisionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IDivisionOperators<uint, uint, uint>.operator /(uint left, uint right)
            => left / right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IDivisionOperators<uint, uint, uint>.operator /(uint left, uint right)
        //     => checked(left / right);

        //
        // IEqualityOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IEqualityOperators<uint, uint>.operator ==(uint left, uint right)
            => left == right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IEqualityOperators<uint, uint>.operator !=(uint left, uint right)
            => left != right;

        //
        // IIncrementOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IIncrementOperators<uint>.operator ++(uint value)
            => ++value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IIncrementOperators<uint>.operator ++(uint value)
        //     => checked(++value);

        //
        // IMinMaxValue
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IMinMaxValue<uint>.MinValue => MinValue;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IMinMaxValue<uint>.MaxValue => MaxValue;

        //
        // IModulusOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IModulusOperators<uint, uint, uint>.operator %(uint left, uint right)
            => left % right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IModulusOperators<uint, uint, uint>.operator %(uint left, uint right)
        //     => checked(left % right);

        //
        // IMultiplicativeIdentity
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IMultiplicativeIdentity<uint, uint>.MultiplicativeIdentity => 1;

        //
        // IMultiplyOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IMultiplyOperators<uint, uint, uint>.operator *(uint left, uint right)
            => left * right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IMultiplyOperators<uint, uint, uint>.operator *(uint left, uint right)
        //     => checked(left * right);

        //
        // INumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.One => 1;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Zero => 0;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Abs(uint value)
            => value;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Clamp(uint value, uint min, uint max)
            => Math.Clamp(value, min, max);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint INumber<uint>.Create<TOther>(TOther value)
        {
            if (typeof(TOther) == typeof(byte))
            {
                return (byte)(object)value;
            }
            else if (typeof(TOther) == typeof(char))
            {
                return (char)(object)value;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                return checked((uint)(decimal)(object)value);
            }
            else if (typeof(TOther) == typeof(double))
            {
                return checked((uint)(double)(object)value);
            }
            else if (typeof(TOther) == typeof(short))
            {
                return checked((uint)(short)(object)value);
            }
            else if (typeof(TOther) == typeof(int))
            {
                return checked((uint)(int)(object)value);
            }
            else if (typeof(TOther) == typeof(long))
            {
                return checked((uint)(long)(object)value);
            }
            else if (typeof(TOther) == typeof(nint))
            {
                return checked((uint)(nint)(object)value);
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return checked((uint)(sbyte)(object)value);
            }
            else if (typeof(TOther) == typeof(float))
            {
                return checked((uint)(float)(object)value);
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                return (ushort)(object)value;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                return (uint)(object)value;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                return checked((uint)(ulong)(object)value);
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return checked((uint)(nuint)(object)value);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint INumber<uint>.CreateSaturating<TOther>(TOther value)
        {
            if (typeof(TOther) == typeof(byte))
            {
                return (byte)(object)value;
            }
            else if (typeof(TOther) == typeof(char))
            {
                return (char)(object)value;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualValue = (short)(object)value;
                return (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualValue = (int)(object)value;
                return (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualValue = (sbyte)(object)value;
                return (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                return (ushort)(object)value;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                return (uint)(object)value;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualValue = (ulong)(object)value;
                return (actualValue > MaxValue) ? MaxValue : (uint)actualValue;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualValue = (nuint)(object)value;
                return (actualValue > MaxValue) ? MaxValue : (uint)actualValue;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint INumber<uint>.CreateTruncating<TOther>(TOther value)
        {
            if (typeof(TOther) == typeof(byte))
            {
                return (byte)(object)value;
            }
            else if (typeof(TOther) == typeof(char))
            {
                return (char)(object)value;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                return (uint)(decimal)(object)value;
            }
            else if (typeof(TOther) == typeof(double))
            {
                return (uint)(double)(object)value;
            }
            else if (typeof(TOther) == typeof(short))
            {
                return (uint)(short)(object)value;
            }
            else if (typeof(TOther) == typeof(int))
            {
                return (uint)(int)(object)value;
            }
            else if (typeof(TOther) == typeof(long))
            {
                return (uint)(long)(object)value;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                return (uint)(nint)(object)value;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return (uint)(sbyte)(object)value;
            }
            else if (typeof(TOther) == typeof(float))
            {
                return (uint)(float)(object)value;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                return (ushort)(object)value;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                return (uint)(object)value;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                return (uint)(ulong)(object)value;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return (uint)(nuint)(object)value;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static (uint Quotient, uint Remainder) INumber<uint>.DivRem(uint left, uint right)
            => Math.DivRem(left, right);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Max(uint x, uint y)
            => Math.Max(x, y);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Min(uint x, uint y)
            => Math.Min(x, y);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Parse(string s, NumberStyles style, IFormatProvider? provider)
            => Parse(s, style, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
            => Parse(s, style, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint INumber<uint>.Sign(uint value)
            => (uint)((value == 0) ? 0 : 1);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumber<uint>.TryCreate<TOther>(TOther value, out uint result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                result = (byte)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                result = (char)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;

                if ((actualValue < 0) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;

                if ((actualValue < 0) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualValue = (short)(object)value;

                if (actualValue < 0)
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualValue = (int)(object)value;

                if (actualValue < 0)
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;

                if ((actualValue < 0) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;

                if ((actualValue < 0) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualValue = (sbyte)(object)value;

                if (actualValue < 0)
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;

                if ((actualValue < 0) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                result = (ushort)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                result = (uint)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualValue = (ulong)(object)value;

                if (actualValue > MaxValue)
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualValue = (nuint)(object)value;

                if (actualValue > MaxValue)
                {
                    result = default;
                    return false;
                }

                result = (uint)actualValue;
                return true;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                result = default;
                return false;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool INumber<uint>.TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out uint result)
            => TryParse(s, style, provider, out result);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool INumber<uint>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out uint result)
            => TryParse(s, style, provider, out result);

        //
        // IParseable
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IParseable<uint>.Parse(string s, IFormatProvider? provider)
            => Parse(s, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IParseable<uint>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out uint result)
            => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // IShiftOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IShiftOperators<uint, uint>.operator <<(uint value, int shiftAmount)
            => value << (int)shiftAmount;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IShiftOperators<uint, uint>.operator >>(uint value, int shiftAmount)
            => value >> (int)shiftAmount;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static uint IShiftOperators<uint, uint>.operator >>>(uint value, int shiftAmount)
        //     => value >> (int)shiftAmount;

        //
        // ISpanParseable
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint ISpanParseable<uint>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
            => Parse(s, NumberStyles.Integer, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool ISpanParseable<uint>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out uint result)
            => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // ISubtractionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint ISubtractionOperators<uint, uint, uint>.operator -(uint left, uint right)
            => left - right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint ISubtractionOperators<uint, uint, uint>.operator -(uint left, uint right)
        //     => checked(left - right);

        //
        // IUnaryNegationOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IUnaryNegationOperators<uint, uint>.operator -(uint value)
            => 0u - value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IUnaryNegationOperators<uint, uint>.operator -(uint value)
        //     => checked(0u - value);

        //
        // IUnaryPlusOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static uint IUnaryPlusOperators<uint, uint>.operator +(uint value)
            => +value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked uint IUnaryPlusOperators<uint, uint>.operator +(uint value)
        //     => checked(+value);
#endif // FEATURE_GENERIC_MATH
    }
}
