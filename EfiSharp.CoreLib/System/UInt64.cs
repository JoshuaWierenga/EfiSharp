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
    public readonly struct UInt64 : /*IComparable, IConvertible, ISpanFormattable, IComparable<ulong>,*/ IEquatable<ulong>
#if FEATURE_GENERIC_MATH //TODO Fix interfaces, are generics broken as well?
#pragma warning disable SA1001, CA2252 // SA1001: Comma positioning; CA2252: Preview Features
        , IBinaryInteger<ulong>,
          IMinMaxValue<ulong>,
          IUnsignedNumber<ulong>
#pragma warning restore SA1001, CA2252
#endif // FEATURE_GENERIC_MATH

    {
        private readonly ulong m_value; // Do not rename (binary serialization)

        public const ulong MaxValue = (ulong)0xffffffffffffffffL;
        public const ulong MinValue = 0x0;

        // Compares this object to another object, returning an integer that
        // indicates the relationship.
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type UInt64, this method throws an ArgumentException.
        //
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return 1;
            }

            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (value is ulong i)
            {
                if (m_value < i) return -1;
                if (m_value > i) return 1;
                return 0;
            }

            throw new ArgumentException(SR.Arg_MustBeUInt64);
        }

        public int CompareTo(ulong value)
        {
            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            return 0;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is ulong))
            {
                return false;
            }
            return m_value == ((ulong)obj).m_value;
        }

        [NonVersionable]
        public bool Equals(ulong obj)
        {
            return m_value == obj;
        }

        // The value of the lower 32 bits XORed with the uppper 32 bits.
        public override int GetHashCode()
        {
            return ((int)m_value) ^ (int)(m_value >> 32);
        }

        public override string ToString()
        {
            return Number.UInt64ToDecStr(m_value, -1);
        }

        public string ToString(IFormatProvider? provider)
        {
            return Number.UInt64ToDecStr(m_value, -1);
        }

        //TODO Add Number.FormatUInt64
        /*public string ToString(string? format)
        {
            return Number.FormatUInt64(m_value, format, null);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            return Number.FormatUInt64(m_value, format, provider);
        }*/

        //TODO Add Span<T>, ReadOnlySpan<T> and Number.TryFormatUInt64
        /*public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            return Number.TryFormatUInt64(m_value, format, provider, destination, out charsWritten);
        }*/

        //TODO Add Number.ParseUInt64, NumberStyles and NumberFormatInfo
        /*public static ulong Parse(string s)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static ulong Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt64(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static ulong Parse(string s, IFormatProvider? provider)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static ulong Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number.ParseUInt64
        /*public static ulong Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add Number.TryParseUInt64IntegerStyle, NumberStyles and NumberFormatInfo
        /*public static bool TryParse([NotNullWhen(true)] string? s, out ulong result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }

            return Number.TryParseUInt64IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add ReadOnlySpan<T>, Number.TryParseUInt64IntegerStyle, NumberStyles and NumberFormatInfo
        /*public static bool TryParse(ReadOnlySpan<char> s, out ulong result)
        {
            return Number.TryParseUInt64IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add NumberStyles, NumberFormatInfo and Number.TryParseUInt64
        /*public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out ulong result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);

            if (s == null)
            {
                result = 0;
                return false;
            }

            return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number.TryParseUInt64
        /*public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out ulong result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }*/

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt64;
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
            return Convert.ToUInt32(m_value);
        }

        long IConvertible.ToInt64(IFormatProvider? provider)
        {
            return Convert.ToInt64(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider)
        {
            return m_value;
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
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "UInt64", "DateTime"));
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
        static ulong IAdditionOperators<ulong, ulong, ulong>.operator +(ulong left, ulong right)
            => left + right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IAdditionOperators<ulong, ulong, ulong>.operator +(ulong left, ulong right)
        //     => checked(left + right);

        //
        // IAdditiveIdentity
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IAdditiveIdentity<ulong, ulong>.AdditiveIdentity => 0;

        //
        // IBinaryInteger
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBinaryInteger<ulong>.LeadingZeroCount(ulong value)
            => (ulong)BitOperations.LeadingZeroCount(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBinaryInteger<ulong>.PopCount(ulong value)
            => (ulong)BitOperations.PopCount(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBinaryInteger<ulong>.RotateLeft(ulong value, int rotateAmount)
            => BitOperations.RotateLeft(value, rotateAmount);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBinaryInteger<ulong>.RotateRight(ulong value, int rotateAmount)
            => BitOperations.RotateRight(value, rotateAmount);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBinaryInteger<ulong>.TrailingZeroCount(ulong value)
            => (ulong)BitOperations.TrailingZeroCount(value);

        //
        // IBinaryNumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IBinaryNumber<ulong>.IsPow2(ulong value)
            => BitOperations.IsPow2(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBinaryNumber<ulong>.Log2(ulong value)
            => (ulong)BitOperations.Log2(value);

        //
        // IBitwiseOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBitwiseOperators<ulong, ulong, ulong>.operator &(ulong left, ulong right)
            => left & right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBitwiseOperators<ulong, ulong, ulong>.operator |(ulong left, ulong right)
            => left | right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBitwiseOperators<ulong, ulong, ulong>.operator ^(ulong left, ulong right)
            => left ^ right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IBitwiseOperators<ulong, ulong, ulong>.operator ~(ulong value)
            => ~value;

        //
        // IComparisonOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<ulong, ulong>.operator <(ulong left, ulong right)
            => left < right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<ulong, ulong>.operator <=(ulong left, ulong right)
            => left <= right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<ulong, ulong>.operator >(ulong left, ulong right)
            => left > right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<ulong, ulong>.operator >=(ulong left, ulong right)
            => left >= right;

        //
        // IDecrementOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IDecrementOperators<ulong>.operator --(ulong value)
            => --value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IDecrementOperators<ulong>.operator --(ulong value)
        //     => checked(--value);

        //
        // IDivisionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IDivisionOperators<ulong, ulong, ulong>.operator /(ulong left, ulong right)
            => left / right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IDivisionOperators<ulong, ulong, ulong>.operator /(ulong left, ulong right)
        //     => checked(left / right);

        //
        // IEqualityOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IEqualityOperators<ulong, ulong>.operator ==(ulong left, ulong right)
            => left == right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IEqualityOperators<ulong, ulong>.operator !=(ulong left, ulong right)
            => left != right;

        //
        // IIncrementOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IIncrementOperators<ulong>.operator ++(ulong value)
            => ++value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IIncrementOperators<ulong>.operator ++(ulong value)
        //     => checked(++value);

        //
        // IMinMaxValue
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IMinMaxValue<ulong>.MinValue => MinValue;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IMinMaxValue<ulong>.MaxValue => MaxValue;

        //
        // IModulusOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IModulusOperators<ulong, ulong, ulong>.operator %(ulong left, ulong right)
            => left % right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IModulusOperators<ulong, ulong, ulong>.operator %(ulong left, ulong right)
        //     => checked(left % right);

        //
        // IMultiplicativeIdentity
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IMultiplicativeIdentity<ulong, ulong>.MultiplicativeIdentity => 1;

        //
        // IMultiplyOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IMultiplyOperators<ulong, ulong, ulong>.operator *(ulong left, ulong right)
            => left * right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IMultiplyOperators<ulong, ulong, ulong>.operator *(ulong left, ulong right)
        //     => checked(left * right);

        //
        // INumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.One => 1;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Zero => 0;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Abs(ulong value)
            => value;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Clamp(ulong value, ulong min, ulong max)
            => Math.Clamp(value, min, max);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong INumber<ulong>.Create<TOther>(TOther value)
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
                return checked((ulong)(decimal)(object)value);
            }
            else if (typeof(TOther) == typeof(double))
            {
                return checked((ulong)(double)(object)value);
            }
            else if (typeof(TOther) == typeof(short))
            {
                return checked((ulong)(short)(object)value);
            }
            else if (typeof(TOther) == typeof(int))
            {
                return checked((ulong)(int)(object)value);
            }
            else if (typeof(TOther) == typeof(long))
            {
                return checked((ulong)(long)(object)value);
            }
            else if (typeof(TOther) == typeof(nint))
            {
                return checked((ulong)(nint)(object)value);
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return checked((ulong)(sbyte)(object)value);
            }
            else if (typeof(TOther) == typeof(float))
            {
                return checked((ulong)(float)(object)value);
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
                return (ulong)(object)value;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return (nuint)(object)value;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong INumber<ulong>.CreateSaturating<TOther>(TOther value)
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
                       (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualValue = (short)(object)value;
                return (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualValue = (int)(object)value;
                return (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;
                return (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;
                return (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualValue = (sbyte)(object)value;
                return (actualValue < 0) ? MinValue : (ulong)actualValue;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < 0) ? MinValue : (ulong)actualValue;
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
                return (ulong)(object)value;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return (nuint)(object)value;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong INumber<ulong>.CreateTruncating<TOther>(TOther value)
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
                return (ulong)(decimal)(object)value;
            }
            else if (typeof(TOther) == typeof(double))
            {
                return (ulong)(double)(object)value;
            }
            else if (typeof(TOther) == typeof(short))
            {
                return (ulong)(short)(object)value;
            }
            else if (typeof(TOther) == typeof(int))
            {
                return (ulong)(int)(object)value;
            }
            else if (typeof(TOther) == typeof(long))
            {
                return (ulong)(long)(object)value;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                return (ulong)(nint)(object)value;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return (ulong)(sbyte)(object)value;
            }
            else if (typeof(TOther) == typeof(float))
            {
                return (ulong)(float)(object)value;
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
                return (ulong)(object)value;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return (nuint)(object)value;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static (ulong Quotient, ulong Remainder) INumber<ulong>.DivRem(ulong left, ulong right)
            => Math.DivRem(left, right);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Max(ulong x, ulong y)
            => Math.Max(x, y);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Min(ulong x, ulong y)
            => Math.Min(x, y);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Parse(string s, NumberStyles style, IFormatProvider? provider)
            => Parse(s, style, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
            => Parse(s, style, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong INumber<ulong>.Sign(ulong value)
            => (ulong)((value == 0) ? 0 : 1);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumber<ulong>.TryCreate<TOther>(TOther value, out ulong result)
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

                if ((actualValue < MinValue) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (ulong)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;

                if ((actualValue < MinValue) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (ulong)actualValue;
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

                result = (ulong)actualValue;
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

                result = (ulong)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;

                if (actualValue < 0)
                {
                    result = default;
                    return false;
                }

                result = (ulong)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;

                if (actualValue < 0)
                {
                    result = default;
                    return false;
                }

                result = (ulong)actualValue;
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

                result = (ulong)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;

                if ((actualValue < MinValue) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (ulong)actualValue;
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
                result = (ulong)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                result = (nuint)(object)value;
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
        static bool INumber<ulong>.TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out ulong result)
            => TryParse(s, style, provider, out result);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool INumber<ulong>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out ulong result)
            => TryParse(s, style, provider, out result);

        //
        // IParseable
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IParseable<ulong>.Parse(string s, IFormatProvider? provider)
            => Parse(s, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IParseable<ulong>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ulong result)
            => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // IShiftOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IShiftOperators<ulong, ulong>.operator <<(ulong value, int shiftAmount)
            => value << (int)shiftAmount;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IShiftOperators<ulong, ulong>.operator >>(ulong value, int shiftAmount)
            => value >> (int)shiftAmount;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static ulong IShiftOperators<ulong, ulong>.operator >>>(ulong value, int shiftAmount)
        //     => value >> (int)shiftAmount;

        //
        // ISpanParseable
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong ISpanParseable<ulong>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
            => Parse(s, NumberStyles.Integer, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool ISpanParseable<ulong>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ulong result)
            => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // ISubtractionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong ISubtractionOperators<ulong, ulong, ulong>.operator -(ulong left, ulong right)
            => left - right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong ISubtractionOperators<ulong, ulong, ulong>.operator -(ulong left, ulong right)
        //     => checked(left - right);

        //
        // IUnaryNegationOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IUnaryNegationOperators<ulong, ulong>.operator -(ulong value)
            => 0UL - value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IUnaryNegationOperators<ulong, ulong>.operator -(ulong value)
        //     => checked(0UL - value);

        //
        // IUnaryPlusOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static ulong IUnaryPlusOperators<ulong, ulong>.operator +(ulong value)
            => +value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked ulong IUnaryPlusOperators<ulong, ulong>.operator +(ulong value)
        //     => checked(+value);
#endif // FEATURE_GENERIC_MATH
    }
}
