// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

// This file contains the basic primitive type definitions (int etc)
// These types are well known to the compiler and the runtime and are basic interchange types that do not change

// CONTRACT with Runtime
// Each of the data types has a data contract with the runtime. See the contract in the type definition
//

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System
{
    // CONTRACT with Runtime
    // Place holder type for type hierarchy, Compiler/Runtime requires this class
    public abstract class ValueType
    {
    }

    // CONTRACT with Runtime, Compiler/Runtime requires this class
    // Place holder type for type hierarchy
    public abstract class Enum : ValueType
    {
    }


    /*============================================================
    **
    ** Class:  Single
    **
    **
    ** Purpose: A wrapper class for the primitive type float.
    **
    **
    ===========================================================*/

    // CONTRACT with Runtime
    // The Single type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type float
    // This type is LayoutKind Sequential

    [StructLayout(LayoutKind.Sequential)]
    public struct Single : IComparable<float>, IEquatable<float>
    {
        private float _value;

        /// <summary>Determines whether the specified value is NaN.</summary>
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsNaN(float f)
        {
            // A NaN will never equal itself so this is an
            // easy and efficient way to check for NaN.

#pragma warning disable CS1718
            return f != f;
#pragma warning restore CS1718
        }

        public int CompareTo(float value)
        {
            if (_value < value) return -1;
            if (_value > value) return 1;
            if (_value == value) return 0;

            // At least one of the values is NaN.
            if (IsNaN(_value))
                return IsNaN(value) ? 0 : -1;
            else // f is NaN.
                return 1;
        }

        public bool Equals(float obj)
        {
            if (obj == _value)
            {
                return true;
            }

            return IsNaN(obj) && IsNaN(_value);
        }
    }


    /*============================================================
    **
    ** Class:  Double
    **
    **
    ** Purpose: A representation of an IEEE double precision
    **          floating point number.
    **
    **
    ===========================================================*/

    // CONTRACT with Runtime
    // The Double type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type double
    // This type is LayoutKind Sequential

    [StructLayout(LayoutKind.Sequential)]
    public struct Double : IComparable<double>, IEquatable<double>
    {
        private double _value;

        /// <summary>Determines whether the specified value is NaN.</summary>
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsNaN(double d)
        {
            // A NaN will never equal itself so this is an
            // easy and efficient way to check for NaN.

#pragma warning disable CS1718
            return d != d;
#pragma warning restore CS1718
        }

        public int CompareTo(double value)
        {
            if (_value < value) return -1;
            if (_value > value) return 1;
            if (_value == value) return 0;

            // At least one of the values is NaN.
            if (IsNaN(_value))
                return IsNaN(value) ? 0 : -1;
            else
                return 1;
        }

        public bool Equals(double obj)
        {
            if (obj == _value)
            {
                return true;
            }
            return IsNaN(obj) && IsNaN(_value);
        }
    }


    /*============================================================
    **
    ** Class:  IntPtr
    **
    **
    ** Purpose: Platform independent integer
    **
    **
    ===========================================================*/

    // CONTRACT with Runtime
    // The IntPtr type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type void *

    // This type implements == without overriding GetHashCode, Equals, disable compiler warning
#pragma warning disable 0660, 0661
    public struct IntPtr
    {
        private unsafe void* _value; // The compiler treats void* closest to uint hence explicit casts are required to preserve int behavior

        [Intrinsic]
        public static readonly IntPtr Zero;

        public static unsafe int Size
        {
            [Intrinsic]
            get
            {
#if TARGET_64BIT
                return 8;
#else
                return 4;
#endif
            }
        }

        [Intrinsic]
        public unsafe IntPtr(void* value)
        {
            _value = value;
        }

        [Intrinsic]
        public unsafe IntPtr(int value)
        {
            _value = (void*)value;
        }

        [Intrinsic]
        public unsafe IntPtr(long value)
        {
            _value = (void*)value;
        }

        [Intrinsic]
        public unsafe long ToInt64()
        {
#if TARGET_64BIT
            return (long)_value;
#else
            return (long)(int)_value;
#endif
        }

        [Intrinsic]
        public static unsafe explicit operator IntPtr(int value)
        {
            return new IntPtr(value);
        }

        [Intrinsic]
        public static unsafe explicit operator IntPtr(long value)
        {
            return new IntPtr(value);
        }

        [Intrinsic]
        public static unsafe explicit operator IntPtr(void* value)
        {
            return new IntPtr(value);
        }

        [Intrinsic]
        public static unsafe explicit operator void*(IntPtr value)
        {
            return value._value;
        }

        [Intrinsic]
        public static unsafe explicit operator int(IntPtr value)
        {
            return unchecked((int)value._value);
        }

        [Intrinsic]
        public static unsafe explicit operator long(IntPtr value)
        {
            return unchecked((long)value._value);
        }

        [Intrinsic]
        public static unsafe bool operator ==(IntPtr value1, IntPtr value2)
        {
            return value1._value == value2._value;
        }

        [Intrinsic]
        public static unsafe bool operator !=(IntPtr value1, IntPtr value2)
        {
            return value1._value != value2._value;
        }

        [Intrinsic]
        public static unsafe IntPtr operator +(IntPtr pointer, int offset)
        {
#if TARGET_64BIT
            return new IntPtr((long)pointer._value + offset);
#else
            return new IntPtr((int)pointer._value + offset);
#endif
        }
    }
#pragma warning restore 0660, 0661


    /*============================================================
    **
    ** Class:  UIntPtr
    **
    **
    ** Purpose: Platform independent integer
    **
    **
    ===========================================================*/

    // CONTRACT with Runtime
    // The UIntPtr type is one of the primitives understood by the compilers and runtime
    // Data Contract: Single field of type void *

    // This type implements == without overriding GetHashCode, Equals, disable compiler warning
#pragma warning disable 0660, 0661
    public struct UIntPtr
    {
        private unsafe void* _value;

        [Intrinsic]
        public static readonly UIntPtr Zero;

        [Intrinsic]
        public unsafe UIntPtr(uint value)
        {
            _value = (void*)value;
        }

        [Intrinsic]
        public unsafe UIntPtr(ulong value)
        {
#if TARGET_64BIT
            _value = (void*)value;
#else
            _value = (void*)checked((uint)value);
#endif
        }

        [Intrinsic]
        public unsafe UIntPtr(void* value)
        {
            _value = value;
        }

        [Intrinsic]
        public static unsafe explicit operator UIntPtr(void* value)
        {
            return new UIntPtr(value);
        }

        [Intrinsic]
        public static unsafe explicit operator void*(UIntPtr value)
        {
            return value._value;
        }

        [Intrinsic]
        public static unsafe explicit operator uint(UIntPtr value)
        {
#if TARGET_64BIT
            return checked((uint)value._value);
#else
            return (uint)value._value;
#endif
        }

        [Intrinsic]
        public static unsafe explicit operator ulong(UIntPtr value)
        {
            return (ulong)value._value;
        }

        [Intrinsic]
        public static unsafe bool operator ==(UIntPtr value1, UIntPtr value2)
        {
            return value1._value == value2._value;
        }

        [Intrinsic]
        public static unsafe bool operator !=(UIntPtr value1, UIntPtr value2)
        {
            return value1._value != value2._value;
        }
    }
#pragma warning restore 0660, 0661
}
