// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.CompilerServices;

namespace System
{
    public static class Console
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine()
        {
            Internal.Console.Write(Environment.NewLine);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(bool value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(char value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(char[] buffer)
        {
            Write(buffer);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(double value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void WriteLine(uint value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(long value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(bool value)
        {
            if (value)
            {
                Internal.Console.Write("True");
            }
            else
            {
                Internal.Console.Write("False");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void Write(char value)
        {
            char* pValue = stackalloc char[2];
            pValue[0] = value;
            pValue[1] = '\0';

            int length = 2;
            int bufferSize = 8;

            byte* pBytes = stackalloc byte[bufferSize];
            int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pValue, length,
                pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
            IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

            Interop.Kernel32.WriteFile(consoleHandle, pBytes, cbytes, out _, IntPtr.Zero);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void Write(char[] buffer)
        {
            if (buffer == null) return;

            fixed (char* pBuffer = buffer)
            {
                int bufferSize = buffer.Length * 4;

                byte* pBytes = stackalloc byte[bufferSize];
                int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pBuffer,
                    buffer.Length, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
                IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

                Interop.Kernel32.WriteFile(consoleHandle, pBytes, cbytes, out _, IntPtr.Zero);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void Write(char[] buffer, int index, int count)
        {
            if (buffer == null || index >= count || index + count > buffer.Length) return;

            fixed (char* pBuffer = &buffer[index])
            {
                int bufferSize = buffer.Length * 4;
                byte* pBytes = stackalloc byte[count];
                int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pBuffer,
                    count, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
                IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

                Interop.Kernel32.WriteFile(consoleHandle, pBytes, cbytes, out _, IntPtr.Zero);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void Write(double value)
        {
            if (value < 0)
            {
                Write('-');
                value = -value;
            }

            //Print integer component of double
            //TODO Check if iLength will be inaccurate if (ulong)value == 0 or 1
            //17 is used since at a maximum, a double can store that many digits in its mantissa
            int iLength = Write((ulong)value, 17);
            int fLength = 17 - iLength;

            //Print decimal component of double
            Write('.');

            //Test for zeros after the decimal point followed by more numbers, if found, pValue will be printed which is a less accurate method but can handle that
            if ((ulong)((value - (ulong)value) * 10) == 0)
            {
                char* pValue = stackalloc char[fLength];
                value -= (ulong)value;
                for (int i = 0; i < fLength; i++)
                {
                    value *= 10;
                    pValue[i] = (char)((ulong)value % 10 + '0');
                }

                int bufferSize = fLength * 4;

                byte* pBytes = stackalloc byte[bufferSize];
                int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pValue,
                    fLength, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
                IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

                Interop.Kernel32.WriteFile(consoleHandle, pBytes, cbytes, out _, IntPtr.Zero);

                return;
            }

            //This method is more accurate since it avoids repeated multiplication of the number but loses zeros at the front of the decimal part
            long tenPower = 10;
            for (int i = 0; i < fLength - 1; i++)
            {
                tenPower *= 10;
            }

            //Retrieve decimal component of mantissa as integer
            ulong fPart = (ulong)((value - (ulong)value) * tenPower);

            //Print decimal component of double
            Write(fPart, fLength);
        }

        //TODO replace length guess with https://stackoverflow.com/a/6092298, the current implementation breaks for both specific values in a way that is probably fixable but I currently have
        //no clue why and because it cannot handle floating point numbers with large exponents that lead to more than nine total digits(still only nine significant figures though)
        //TODO Once more features are supported, add something like https://github.com/Ninds/Ryu.NET instead of either of these methods
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void Write(float value)
        {
            if (value < 0)
            {
                Write('-');
                value = -value;
            }

            //Print integer component of float
            //TODO Check if iLength will be inaccurate if (ulong)value == 0 or 1
            //9 is used since at a maximum, a float can store that many digits in its mantissa
            int iLength = Write((ulong)value, 9);
            int fLength = 9 - iLength;

            //Print decimal component of float
            Write('.');

            //Test for zeros after the decimal point followed by more numbers, if found, pValue will be printed which is a less accurate method but can handle that
            if ((uint)((value - (uint)value) * 10) == 0)
            {
                char* pValue = stackalloc char[fLength];
                value -= (uint)value;
                for (int i = 0; i < fLength; i++)
                {
                    value *= 10;
                    pValue[i] = (char)((uint)value % 10 + '0');
                }

                int bufferSize = fLength * 4;

                byte* pBytes = stackalloc byte[bufferSize];
                int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pValue,
                    fLength, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
                IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

                Interop.Kernel32.WriteFile(consoleHandle, pBytes, cbytes, out _, IntPtr.Zero);
                return;
            }

            //This method is more accurate since it avoids repeated multiplication of the number but loses zeros at the front of the decimal part
            int tenPower = 10;
            for (int i = 0; i < fLength - 1; i++)
            {
                tenPower *= 10;
            }

            //Retrieve decimal component of mantissa as integer
            uint fPart = (uint)((value - (uint)value) * tenPower);
            //uint fPart2 = (uint)(value * tenPower - (uint)value * tenPower);

            //Print decimal component of float
            Write(fPart, fLength);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(int value)
        {
            //This is needed to prevent value overflowing for -value being >int.MaxValue, I tried simply adding Write((uint)(-value), 1)); but that fails for all negative numbers.
            uint unsignedValue = (uint)value;

            if (value < 0)
            {
                Write('-');
                unsignedValue = (uint)(-value);
            }

            Write(unsignedValue, 10);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(uint value)
        {
            Write(value, 10);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(long value)
        {
            if (value < 0)
            {
                Write('-');
                value = -value;
            }

            Write((ulong)value, 20);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(ulong value)
        {
            Write(value, 20);
        }

        private static unsafe int Write(ulong value, int decimalLength)
        {
            char* pValue = stackalloc char[decimalLength + 1];
            sbyte digitPosition = (sbyte)(decimalLength - 1); //This is designed to go negative for numbers with decimalLength digits

            do
            {
                pValue[digitPosition--] = (char)(value % 10 + '0');
                value /= 10;
            } while (value > 0);

            //digitPosition ends up as decimalLength - 1 - length
            int length = decimalLength - digitPosition - 1;
            int bufferSize = length * 4;

            byte* pBytes = stackalloc byte[bufferSize];
            int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0,
                &pValue[digitPosition + 1], length, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
            IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

            Interop.Kernel32.WriteFile(consoleHandle, pBytes, cbytes, out _, IntPtr.Zero);

            return length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(string value)
        {
            Internal.Console.Write(value);
        }
    }
}
