// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

// Some routines inspired by the Stanford Bit Twiddling Hacks by Sean Eron Anderson:
// http://graphics.stanford.edu/~seander/bithacks.html

using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerServices;

namespace System.Numerics
{
    // <summary>
    /// Utility methods for intrinsic bit-twiddling operations.
    /// The methods use hardware intrinsics when available on the underlying platform,
    /// otherwise they use optimized software fallbacks.
    /// </summary>
    //TODO Add more of file
    public static class BitOperations
    {
        //TODO Add ReadOnlySpan<T>
        //private static ReadOnlySpan<byte> Log2DeBruijn => new byte[32]
        private static byte[] Log2DeBruijn => new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        /// <summary>
        /// Returns the integer (floor) log of the specified value, base 2.
        /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        public static int Log2(uint value)
        {
            // The 0->0 contract is fulfilled by setting the LSB to 1.
            // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
            value |= 1;

            // value    lzcnt   actual  expected
            // ..0001   31      31-31    0
            // ..0010   30      31-30    1
            // 0010..    2      31-2    29
            // 0100..    1      31-1    30
            // 1000..    0      31-0    31
            //TODO Support Intrinsics
            /*if (Lzcnt.IsSupported)
            {
                return 31 ^ (int)Lzcnt.LeadingZeroCount(value);
            }

            if (ArmBase.IsSupported)
            {
                return 31 ^ ArmBase.LeadingZeroCount(value);
            }

            // BSR returns the log2 result directly.However BSR is slower than LZCNT
            // on AMD processors, so we leave it as a fallback only.
            if (X86Base.IsSupported)
            {
                return (int)X86Base.BitScanReverse(value);
            }*/

            // Fallback contract is 0->0
            return Log2SoftwareFallback(value);
        }

        /// <summary>
        /// Returns the integer (floor) log of the specified value, base 2.
        /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        public static int Log2(ulong value)
        {
            value |= 1;

            //TODO Support Intrinsics
            /*if (Lzcnt.X64.IsSupported)
            {
                return 63 ^ (int)Lzcnt.X64.LeadingZeroCount(value);
            }

            if (ArmBase.Arm64.IsSupported)
            {
                return 63 ^ ArmBase.Arm64.LeadingZeroCount(value);
            }

            if (X86Base.X64.IsSupported)
            {
                return (int)X86Base.X64.BitScanReverse(value);
            }*/

            uint hi = (uint)(value >> 32);

            if (hi == 0)
            {
                return Log2((uint)value);
            }

            return 32 + Log2(hi);
        }

        /// <summary>
        /// Returns the integer (floor) log of the specified value, base 2.
        /// Note that by convention, input value 0 returns 0 since Log(0) is undefined.
        /// Does not directly use any hardware intrinsics, nor does it incur branching.
        /// </summary>
        /// <param name="value">The value.</param>
        private static int Log2SoftwareFallback(uint value)
        {
            // No AggressiveInlining due to large method size
            // Has conventional contract 0->0 (Log(0) is undefined)

            // Fill trailing zeros with ones, eg 00010010 becomes 00011111
            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            return Unsafe.AddByteOffset(
                // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_1100_0100_1010_1100_1101_1101u
                //TODO Add ReadOnlySpan<T> and MemoryMarshal.GetReference
                //ref MemoryMarshal.GetReference(Log2DeBruijn),
                ref Log2DeBruijn[0],
                // uint|long -> IntPtr cast on 32-bit platforms does expensive overflow checks not needed here
                (IntPtr)(int)((value * 0x07C4ACDDu) >> 27));
        }

        /// <summary>Returns the integer (ceiling) log of the specified value, base 2.</summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2Ceiling(ulong value)
        {
            int result = Log2(value);
            if (PopCount(value) != 1)
            {
                result++;
            }
            return result;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The value.</param>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        public static int PopCount(ulong value)
        {
            //TODO Support Intrinsics, Vector64.Create(ulong), Vector64<ulong>.AsByte() and Vector64<T>
            /*if (Popcnt.X64.IsSupported)
            {
                return (int)Popcnt.X64.PopCount(value);
            }

            if (AdvSimd.Arm64.IsSupported)
            {
                // PopCount works on vector so convert input value to vector first.
                Vector64<ulong> input = Vector64.Create(value);
                Vector64<byte> aggregated = AdvSimd.Arm64.AddAcross(AdvSimd.PopCount(input.AsByte()));
                return aggregated.ToScalar();
            }*/

#if TARGET_32BIT
            return PopCount((uint)value) // lo
                + PopCount((uint)(value >> 32)); // hi
#else
            return SoftwareFallback(value);

            static int SoftwareFallback(ulong value)
            {
                const ulong c1 = 0x_55555555_55555555ul;
                const ulong c2 = 0x_33333333_33333333ul;
                const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
                const ulong c4 = 0x_01010101_01010101ul;

                value -= (value >> 1) & c1;
                value = (value & c2) + ((value >> 2) & c2);
                value = (((value + (value >> 4)) & c3) * c4) >> 56;

                return (int)value;
            }
#endif
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        public static ulong RotateLeft(ulong value, int offset)
            => (value << offset) | (value >> (64 - offset));
    }
}