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

        private static unsafe void Write(ulong value, int decimalLength)
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
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(string value)
        {
            Internal.Console.Write(value);
        }
    }
}
