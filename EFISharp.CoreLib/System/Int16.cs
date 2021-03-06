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
    public readonly struct Int16 :/* IComparable, IConvertible, ISpanFormattable, IComparable<short>,*/ IEquatable<short>
    {
        private readonly short m_value; // Do not rename (binary serialization)

        public const short MaxValue = (short)0x7FFF;
        public const short MinValue = unchecked((short)0x8000);

        // Compares this object to another object, returning an integer that
        // indicates the relationship.
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type Int16, this method throws an ArgumentException.
        //
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value is short)
            {
                return m_value - ((short)value).m_value;
            }

            throw new ArgumentException(SR.Arg_MustBeInt16);
        }

        public int CompareTo(short value)
        {
            return m_value - value;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is short))
            {
                return false;
            }
            return m_value == ((short)obj).m_value;
        }

        [NonVersionable]
        public bool Equals(short obj)
        {
            return m_value == obj;
        }

        // Returns a HashCode for the Int16
        public override int GetHashCode()
        {
            return m_value;
        }

        public override string ToString()
        {
            return Number.Int32ToDecStr(m_value);
        }

        //TODO Add Number.FormatInt32
        /*public string ToString(IFormatProvider? provider)
        {
            return Number.FormatInt32(m_value, 0, null, provider);
        }

        public string ToString(string? format)
        {
            return ToString(format, null);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            return Number.FormatInt32(m_value, 0x0000FFFF, format, provider);
        }*/

        //TODO Add Span<T>, ReadOnlySpan<T> and Number.TryFormatInt32
        /*public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            return Number.TryFormatInt32(m_value, 0x0000FFFF, format, provider, destination, out charsWritten);
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles and NumberFormatInfo
        /*public static short Parse(string s)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Parse((ReadOnlySpan<char>)s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Parse((ReadOnlySpan<char>)s, style, NumberFormatInfo.CurrentInfo);
        }

        public static short Parse(string s, IFormatProvider? provider)
        {
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Parse((ReadOnlySpan<char>)s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static short Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
            return Parse((ReadOnlySpan<char>)s, style, NumberFormatInfo.GetInstance(provider));
        }

        public static short Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo, Number.ThrowOverflowOrFormatException and Number.ThrowOverflowException
        /*private static short Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
        {
            Number.ParsingStatus status = Number.TryParseInt32(s, style, info, out int i);
            if (status != Number.ParsingStatus.OK)
            {
                Number.ThrowOverflowOrFormatException(status, TypeCode.Int16);
            }

            // For hex number styles AllowHexSpecifier << 6 == 0x8000 and cancels out MinValue so the check is effectively: (uint)i > ushort.MaxValue
            // For integer styles it's zero and the effective check is (uint)(i - MinValue) > ushort.MaxValue
            if ((uint)(i - MinValue - ((int)(style & NumberStyles.AllowHexSpecifier) << 6)) > ushort.MaxValue)
            {
                Number.ThrowOverflowException(TypeCode.Int16);
            }
            return (short)i;
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles and NumberFormatInfo
        /*public static bool TryParse([NotNullWhen(true)] string? s, out short result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }

            return TryParse((ReadOnlySpan<char>)s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out short result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out short result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);

            if (s == null)
            {
                result = 0;
                return false;
            }

            return TryParse((ReadOnlySpan<char>)s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out short result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }*/

        //TODO Add ReadOnlySpan<T>, NumberStyles, NumberFormatInfo and Number
        /*private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out short result)
        {
            // For hex number styles AllowHexSpecifier << 6 == 0x8000 and cancels out MinValue so the check is effectively: (uint)i > ushort.MaxValue
            // For integer styles it's zero and the effective check is (uint)(i - MinValue) > ushort.MaxValue
            if (Number.TryParseInt32(s, style, info, out int i) != Number.ParsingStatus.OK
                || (uint)(i - MinValue - ((int)(style & NumberStyles.AllowHexSpecifier) << 6)) > ushort.MaxValue)
            {
                result = 0;
                return false;
            }
            result = (short)i;
            return true;
        }*/

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int16;
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
            return m_value;
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
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Int16", "DateTime"));
        }*/

        //TODO Add IConvertible and Convert
        /*object IConvertible.ToType(Type type, IFormatProvider? provider)
        {
            return Convert.DefaultToType((IConvertible)this, type, provider);
        }*/
    }
}
