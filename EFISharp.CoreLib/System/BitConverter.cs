// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using Internal.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Converts base data types to an array of bytes, and an array of bytes to base data types.
    /// </summary>
    public static class BitConverter
    {
        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <!--<exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 3,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>-->
        public static int ToInt32(byte[] value, int startIndex)
        {
            if (value == null)
                //ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
                return 0;
            if (unchecked((uint)startIndex) >= unchecked((uint)value.Length))
                //ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
                return 0;
            if (startIndex > value.Length - sizeof(int))
                //ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall, ExceptionArgument.value);
                return 0;

            //return Unsafe.ReadUnaligned<int>(ref value[startIndex]);
            return Unsafe.As<byte, int>(ref value[startIndex]);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <!--<exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 7,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>-->
        public static long ToInt64(byte[] value, int startIndex)
        {
            if (value == null)
                //ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
                return 0;
            if (unchecked((uint)startIndex) >= unchecked((uint)value.Length))
                //ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
                return 0;
            if (startIndex > value.Length - sizeof(long))
                //ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall, ExceptionArgument.value);
                return 0;

            //return Unsafe.ReadUnaligned<long>(ref value[startIndex]);
            return Unsafe.As<byte, long>(ref value[startIndex]);
        }
    }
}
