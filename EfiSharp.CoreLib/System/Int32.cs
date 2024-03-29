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
    [StructLayout(LayoutKind.Sequential)]
    [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    public readonly struct Int32 : /*IComparable, IConvertible, ISpanFormattable, IComparable<int>,*/ IEquatable<int>
#if FEATURE_GENERIC_MATH //TODO Fix interfaces, are generics broken as well?
#pragma warning disable SA1001, CA2252 // SA1001: Comma positioning; CA2252: Preview Features
        , IBinaryInteger<int>,
          IMinMaxValue<int>,
          ISignedNumber<int>
#pragma warning restore SA1001, CA2252
#endif // FEATURE_GENERIC_MATH
    {
        private readonly int m_value; // Do not rename (binary serialization)

        public const int MaxValue = 0x7fffffff;
        public const int MinValue = unchecked((int)0x80000000);

        // Compares this object to another object, returning an integer that
        // indicates the relationship.
        // Returns :
        // 0 if the values are equal
        // Negative number if _value is less than value
        // Positive number if _value is more than value
        // null is considered to be less than any instance, hence returns positive number
        // If object is not of type Int32, this method throws an ArgumentException.
        //
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return 1;
            }

            // NOTE: Cannot use return (_value - value) as this causes a wrap
            // around in cases where _value - value > MaxValue.
            if (value is int i)
            {
                if (m_value < i) return -1;
                if (m_value > i) return 1;
                return 0;
            }

            throw new ArgumentException(SR.Arg_MustBeInt32);
        }

        public int CompareTo(int value)
        {
            // NOTE: Cannot use return (_value - value) as this causes a wrap
            // around in cases where _value - value > MaxValue.
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            return 0;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is int))
            {
                return false;
            }
            return m_value == ((int)obj).m_value;
        }

        [NonVersionable]
        public bool Equals(int obj)
        {
            return m_value == obj;
        }

        // The absolute value of the int contained.
        public override int GetHashCode()
        {
            return m_value;
        }

        public override string ToString()
        {
            return Number.Int32ToDecStr(m_value);
        }

        //TODO Add Number.FormatInt32
        /*public string ToString(string? format)
        {
            return ToString(format, null);
        }

        public string ToString(IFormatProvider? provider)
        {
            return Number.FormatInt32(m_value, 0, null, provider);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            return Number.FormatInt32(m_value, ~0, format, provider);
        }*/

        //TODO Add Span<T>, ReadOnlySpan<T> and Number.TryFormatInt32
        /*public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            return Number.TryFormatInt32(m_value, ~0, format, provider, destination, out charsWritten);
        }*/

        //TODO Add Number.ParseInt32, NumberStyles and NumberFormatInfo
        /*public static int Parse(string s)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseInt32(s, style, NumberFormatInfo.CurrentInfo);
        }

        // Parses an integer from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's
        // NumberFormatInfo is assumed.
        //
        public static int Parse(string s, IFormatProvider? provider)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        // Parses an integer from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's
        // NumberFormatInfo is assumed.
        //
        public static int Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number.ParseInt32
        /*public static int Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add Number.TryParseInt32IntegerStyle, NumberStyles and NumberFormatInfo
        // Parses an integer from a String. Returns false rather
        // than throwing an exception if input is invalid.
        //
        /*public static bool TryParse([NotNullWhen(true)] string? s, out int result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }

            return Number.TryParseInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add ReadOnlySpan<T>, Number.TryParseInt32IntegerStyle, NumberStyles and NumberFormatInfo
        /*public static bool TryParse(ReadOnlySpan<char> s, out int result)
        {
            return Number.TryParseInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add NumberStyles, NumberFormatInfo and Number
        // Parses an integer from a String in the given style. Returns false rather
        // than throwing an exception if input is invalid.
        //
        /*public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out int result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);

            if (s == null)
            {
                result = 0;
                return false;
            }

            return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number
        /*public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out int result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }*/

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
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
            return m_value;
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
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Int32", "DateTime"));
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
        static int IAdditionOperators<int, int, int>.operator +(int left, int right)
            => left + right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IAdditionOperators<int, int, int>.operator +(int left, int right)
        //     => checked(left + right);

        //
        // IAdditiveIdentity
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IAdditiveIdentity<int, int>.AdditiveIdentity => 0;

        //
        // IBinaryInteger
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBinaryInteger<int>.LeadingZeroCount(int value)
            => BitOperations.LeadingZeroCount((uint)value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBinaryInteger<int>.PopCount(int value)
            => BitOperations.PopCount((uint)value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBinaryInteger<int>.RotateLeft(int value, int rotateAmount)
            => (int)BitOperations.RotateLeft((uint)value, rotateAmount);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBinaryInteger<int>.RotateRight(int value, int rotateAmount)
            => (int)BitOperations.RotateRight((uint)value, rotateAmount);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBinaryInteger<int>.TrailingZeroCount(int value)
            => BitOperations.TrailingZeroCount(value);

        //
        // IBinaryNumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IBinaryNumber<int>.IsPow2(int value)
            => BitOperations.IsPow2(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBinaryNumber<int>.Log2(int value)
        {
            if (value < 0)
            {
                ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
            }
            return BitOperations.Log2((uint)value);
        }

        //
        // IBitwiseOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBitwiseOperators<int, int, int>.operator &(int left, int right)
            => left & right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBitwiseOperators<int, int, int>.operator |(int left, int right)
            => left | right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBitwiseOperators<int, int, int>.operator ^(int left, int right)
            => left ^ right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IBitwiseOperators<int, int, int>.operator ~(int value)
            => ~value;

        //
        // IComparisonOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<int, int>.operator <(int left, int right)
            => left < right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<int, int>.operator <=(int left, int right)
            => left <= right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<int, int>.operator >(int left, int right)
            => left > right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IComparisonOperators<int, int>.operator >=(int left, int right)
            => left >= right;

        //
        // IDecrementOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IDecrementOperators<int>.operator --(int value)
            => --value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IDecrementOperators<int>.operator --(int value)
        //     => checked(--value);

        //
        // IDivisionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IDivisionOperators<int, int, int>.operator /(int left, int right)
            => left / right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IDivisionOperators<int, int, int>.operator /(int left, int right)
        //     => checked(left / right);

        //
        // IEqualityOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IEqualityOperators<int, int>.operator ==(int left, int right)
            => left == right;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IEqualityOperators<int, int>.operator !=(int left, int right)
            => left != right;

        //
        // IIncrementOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IIncrementOperators<int>.operator ++(int value)
            => ++value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IIncrementOperators<int>.operator ++(int value)
        //     => checked(++value);

        //
        // IMinMaxValue
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IMinMaxValue<int>.MinValue => MinValue;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IMinMaxValue<int>.MaxValue => MaxValue;

        //
        // IModulusOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IModulusOperators<int, int, int>.operator %(int left, int right)
            => left % right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IModulusOperators<int, int, int>.operator %(int left, int right)
        //     => checked(left % right);

        //
        // IMultiplicativeIdentity
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IMultiplicativeIdentity<int, int>.MultiplicativeIdentity => 1;

        //
        // IMultiplyOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IMultiplyOperators<int, int, int>.operator *(int left, int right)
            => left * right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IMultiplyOperators<int, int, int>.operator *(int left, int right)
        //     => checked(left * right);

        //
        // INumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.One => 1;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Zero => 0;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Abs(int value)
            => Math.Abs(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Clamp(int value, int min, int max)
            => Math.Clamp(value, min, max);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        internal static int Create<TOther>(TOther value)
            where TOther : INumber<TOther>
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
                return checked((int)(decimal)(object)value);
            }
            else if (typeof(TOther) == typeof(double))
            {
                return checked((int)(double)(object)value);
            }
            else if (typeof(TOther) == typeof(short))
            {
                return (short)(object)value;
            }
            else if (typeof(TOther) == typeof(int))
            {
                return (int)(object)value;
            }
            else if (typeof(TOther) == typeof(long))
            {
                return checked((int)(long)(object)value);
            }
            else if (typeof(TOther) == typeof(nint))
            {
                return checked((int)(nint)(object)value);
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return (sbyte)(object)value;
            }
            else if (typeof(TOther) == typeof(float))
            {
                return checked((int)(float)(object)value);
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                return checked((int)(ushort)(object)value);
            }
            else if (typeof(TOther) == typeof(uint))
            {
                return checked((int)(uint)(object)value);
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                return checked((int)(ulong)(object)value);
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return checked((int)(nuint)(object)value);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int INumber<int>.Create<TOther>(TOther value)
            => Create(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int INumber<int>.CreateSaturating<TOther>(TOther value)
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
                       (actualValue < MinValue) ? MinValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < MinValue) ? MinValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(short))
            {
                return (short)(object)value;
            }
            else if (typeof(TOther) == typeof(int))
            {
                return (int)(object)value;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < MinValue) ? MinValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < MinValue) ? MinValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return (sbyte)(object)value;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                return (actualValue > MaxValue) ? MaxValue :
                       (actualValue < MinValue) ? MinValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                return (ushort)(object)value;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualValue = (uint)(object)value;
                return (actualValue > MaxValue) ? MaxValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualValue = (ulong)(object)value;
                return (actualValue > MaxValue) ? MaxValue : (int)actualValue;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualValue = (nuint)(object)value;
                return (actualValue > MaxValue) ? MaxValue : (int)actualValue;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int INumber<int>.CreateTruncating<TOther>(TOther value)
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
                return (int)(decimal)(object)value;
            }
            else if (typeof(TOther) == typeof(double))
            {
                return (int)(double)(object)value;
            }
            else if (typeof(TOther) == typeof(short))
            {
                return (short)(object)value;
            }
            else if (typeof(TOther) == typeof(int))
            {
                return (int)(object)value;
            }
            else if (typeof(TOther) == typeof(long))
            {
                return (int)(long)(object)value;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                return (int)(nint)(object)value;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                return (sbyte)(object)value;
            }
            else if (typeof(TOther) == typeof(float))
            {
                return (int)(float)(object)value;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                return (ushort)(object)value;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                return (int)(uint)(object)value;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                return (int)(ulong)(object)value;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                return (int)(nuint)(object)value;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static (int Quotient, int Remainder) INumber<int>.DivRem(int left, int right)
            => Math.DivRem(left, right);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Max(int x, int y)
            => Math.Max(x, y);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Min(int x, int y)
            => Math.Min(x, y);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Parse(string s, NumberStyles style, IFormatProvider? provider)
            => Parse(s, style, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
            => Parse(s, style, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int INumber<int>.Sign(int value)
            => Math.Sign(value);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumber<int>.TryCreate<TOther>(TOther value, out int result)
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

                result = (int)actualValue;
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

                result = (int)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                result = (short)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                result = (int)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;

                if ((actualValue < MinValue) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (int)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;

                if ((actualValue < MinValue) || (actualValue > MaxValue))
                {
                    result = default;
                    return false;
                }

                result = (int)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                result = (sbyte)(object)value;
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

                result = (int)actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                result = (ushort)(object)value;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualValue = (uint)(object)value;

                if (actualValue > MaxValue)
                {
                    result = default;
                    return false;
                }

                result = (int)actualValue;
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

                result = (int)actualValue;
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

                result = (int)actualValue;
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
        static bool INumber<int>.TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out int result)
            => TryParse(s, style, provider, out result);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool INumber<int>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out int result)
            => TryParse(s, style, provider, out result);

        //
        // IParseable
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IParseable<int>.Parse(string s, IFormatProvider? provider)
            => Parse(s, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool IParseable<int>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out int result)
            => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // IShiftOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IShiftOperators<int, int>.operator <<(int value, int shiftAmount)
            => value << shiftAmount;

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IShiftOperators<int, int>.operator >>(int value, int shiftAmount)
            => value >> shiftAmount;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static int IShiftOperators<int, int>.operator >>>(int value, int shiftAmount)
        //     => (int)((uint)value >> shiftAmount);

        //
        // ISignedNumber
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int ISignedNumber<int>.NegativeOne => -1;

        //
        // ISpanParseable
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int ISpanParseable<int>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
            => Parse(s, NumberStyles.Integer, provider);

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static bool ISpanParseable<int>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out int result)
            => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // ISubtractionOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int ISubtractionOperators<int, int, int>.operator -(int left, int right)
            => left - right;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int ISubtractionOperators<int, int, int>.operator -(int left, int right)
        //     => checked(left - right);

        //
        // IUnaryNegationOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IUnaryNegationOperators<int, int>.operator -(int value)
            => -value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IUnaryNegationOperators<int, int>.operator -(int value)
        //     => checked(-value);

        //
        // IUnaryPlusOperators
        //

        [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        static int IUnaryPlusOperators<int, int>.operator +(int value)
            => +value;

        // [RequiresPreviewFeatures(Number.PreviewFeatureMessage, Url = Number.PreviewFeatureUrl)]
        // static checked int IUnaryPlusOperators<int, int>.operator +(int value)
        //     => checked(+value);
#endif // FEATURE_GENERIC_MATH
    }
}
