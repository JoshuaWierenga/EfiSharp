// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.IO;
using System.Runtime.CompilerServices;

namespace System
{
    public static unsafe class Console
    {
        //Read and ReadLine
        //sizeof(ReadKey) = 3 bytes => sizeof(_buffer) = 1.536kb
        private static ConsoleReadKey* _buffer;
        private static ushort _bufferIndex;
        private static ushort _bufferLength;
        private const ushort BufferCapacity = 512;
        private static bool readLine;

        static Console()
        {
            IntPtr consoleBuffer = Interop.Kernel32.CreateFile_IntPtr("CONOUT$",
                Interop.Kernel32.GenericOperations.GENERIC_WRITE, FileShare.None, FileMode.Open,
                Interop.Kernel32.FileAttributes.FILE_ATTRIBUTE_NORMAL);

            Interop.Kernel32.GetConsoleCursorInfo(consoleBuffer, out Interop.Kernel32.CONSOLE_CURSOR_INFO cursorInfo);
            cursorInfo.bVisible = Interop.BOOL.TRUE;
            Interop.Kernel32.SetConsoleCursorInfo(consoleBuffer, ref cursorInfo);

            Interop.Kernel32.CloseHandle(consoleBuffer);
        }

        private const int ShiftVKCode = 0x10; // VK_SHIFT
        private const int ControlVKCode = 0x11; // VK_CONTROL
        private const int AltVKCode = 0x12; // VK_MENU

        /*public static ConsoleKeyInfo ReadKey()
        {
            return ReadKey(false);
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_INPUT_HANDLE);
            char* x = stackalloc char[2];
            x[1] = '\0';

            //TODO Figure out how to return on first key pressed instead of on enter, https://docs.microsoft.com/en-us/windows/console/setconsolemode?
            Interop.Kernel32.ReadConsole(consoleHandle, (byte*)x, 1, out _, IntPtr.Zero);

            if (!intercept)
            {
                switch ((ConsoleKey)x[0])
                {
                    case ConsoleKey.Backspace:
                        CursorLeft--;
                        break;
                    case ConsoleKey.Tab:
                        //Moves to next multiple of 8
                        CursorLeft = 8 * (CursorLeft / 8 + 1);
                        break;
                    default:
                        Write(x[0]);
                        break;
                }
            }

            //TODO Support other unicode blocks or other chars supported by uefi?
            if (x[0] >= 127)
            {
                return new ConsoleKeyInfo(x[0], 0, false, false, false);
            }

            //Table used to convert between Unicode Basic Latin characters and those in ConsoleKey, https://unicode-table.com/en/blocks/basic-latin/
            //TODO Fix hanging from using reference type static fields
            ConsoleKey[] keyMap = new ConsoleKey[128]
            {
                //C0 controls
                0, //0
                0, //1
                0, //2
                0, //3
                0, //4
                0, //5
                0, //6
                0, //7
                ConsoleKey.Backspace, //8
                ConsoleKey.Tab, //9
                0, //A
                0, //B
                0, //C
                ConsoleKey.Enter, //D
                0, //E
                0, //F
                0, //10
                0, //11
                0, //12
                0, //13
                0, //14
                0, //15
                0, //16
                0, //17
                0, //18
                0, //19
                0, //1A
                0, //1B
                0, //1C
                0, //1D
                0, //1E
                0, //1F
                //ASCII punctuation and symbols
                ConsoleKey.Spacebar, //20
                ConsoleKey.D1, //21, !, shift
                ConsoleKey.Oem7, //22, ", shift
                ConsoleKey.D3, //23, #, shift
                ConsoleKey.D4, //24, $, shift
                ConsoleKey.D5, //25, %, shift
                ConsoleKey.D7, //26, &, shift
                ConsoleKey.Oem7, //27, '
                ConsoleKey.D9, //28, (, shift
                ConsoleKey.D0, //29, ), shift
                ConsoleKey.D8, //2A, *, shift
                ConsoleKey.OemPlus, //2B, +, shift
                ConsoleKey.OemComma, //2C, ','
                ConsoleKey.OemMinus, //2D, -
                ConsoleKey.OemPeriod, //2E, .
                ConsoleKey.Oem2, //2F, /
                //ASCII digits
                ConsoleKey.D0, //30
                ConsoleKey.D1, //31
                ConsoleKey.D2, //32
                ConsoleKey.D3, //33
                ConsoleKey.D4, //34
                ConsoleKey.D5, //35
                ConsoleKey.D6, //36
                ConsoleKey.D7, //37
                ConsoleKey.D8, //38
                ConsoleKey.D9, //39
                //ASCII punctuation and symbols
                ConsoleKey.Oem1, //3A, :, shift
                ConsoleKey.Oem1, //3B, ;
                ConsoleKey.OemComma, //3C, <, shift
                ConsoleKey.OemPlus, //3D, =
                ConsoleKey.OemPeriod, //3E, >, shift
                ConsoleKey.Oem2, //3F, ?, shift
                ConsoleKey.D2, //40, @, shift
                //Uppercase Latin alphabet
                ConsoleKey.A, //41, shift
                ConsoleKey.B, //42, shift
                ConsoleKey.C, //43, shift
                ConsoleKey.D, //44, shift
                ConsoleKey.E, //45, shift
                ConsoleKey.F, //46, shift
                ConsoleKey.G, //47, shift
                ConsoleKey.H, //48, shift
                ConsoleKey.I, //49, shift
                ConsoleKey.J, //4A, shift
                ConsoleKey.K, //4B, shift
                ConsoleKey.L, //4C, shift
                ConsoleKey.M, //4D, shift
                ConsoleKey.N, //4E, shift
                ConsoleKey.O, //4F, shift
                ConsoleKey.P, //50, shift
                ConsoleKey.Q, //51, shift
                ConsoleKey.R, //52, shift
                ConsoleKey.S, //53, shift
                ConsoleKey.T, //54, shift
                ConsoleKey.U, //55, shift
                ConsoleKey.V, //56, shift
                ConsoleKey.W, //57, shift
                ConsoleKey.X, //58, shift
                ConsoleKey.Y, //59, shift
                ConsoleKey.Z, //5A, shift
                //ASCII punctuation and symbols
                ConsoleKey.Oem4, //5B, [
                ConsoleKey.Oem5, //5C, \
                ConsoleKey.Oem6, //5D, ]
                ConsoleKey.D6, //5E, ^, shift
                ConsoleKey.OemMinus, //5F, _, shift
                ConsoleKey.Oem3, //60, `
                //Lowercase Latin alphabet
                ConsoleKey.A, //61
                ConsoleKey.B, //62
                ConsoleKey.C, //63
                ConsoleKey.D, //64
                ConsoleKey.E, //65
                ConsoleKey.F, //66
                ConsoleKey.G, //67
                ConsoleKey.H, //68
                ConsoleKey.I, //69
                ConsoleKey.J, //6A
                ConsoleKey.K, //6B
                ConsoleKey.L, //6C
                ConsoleKey.M, //6D
                ConsoleKey.N, //6E
                ConsoleKey.O, //6F
                ConsoleKey.P, //70
                ConsoleKey.Q, //71
                ConsoleKey.R, //72
                ConsoleKey.S, //73
                ConsoleKey.T, //74
                ConsoleKey.U, //75
                ConsoleKey.V, //76
                ConsoleKey.W, //77
                ConsoleKey.X, //78
                ConsoleKey.Y, //79
                ConsoleKey.Z, //7A
                //ASCII punctuation and symbols
                ConsoleKey.Oem4, //7B, {, shift
                ConsoleKey.Oem5, //7C, |, shift
                ConsoleKey.Oem6, //7D, }, shift
                ConsoleKey.Oem3, //7E, ~, shift
                //Control character
                0 //7F
            };

            ConsoleKey key = keyMap[x[0]];
            //TODO Figure out why these crash the program
            //bool shift = (Interop.User32.GetKeyState(ShiftVKCode) & 1) == 1;
            //bool alt = (Interop.User32.GetKeyState(AltVKCode) & 1) == 1;
            //bool control = (Interop.User32.GetKeyState(ControlVKCode) & 1) == 1;
            bool shift = false, alt = false, control = false;

            //TODO Replace with Array of ConsoleKeys where the index corresponds with the unicode char
            //and the value is the corresponding ConsoleKey. The conversion is too complex for the switch
            //statement given here. Also need to figure out how to deal with different layouts, is it even 
            //possible if keys outside Basic Latin are used. Are the supplements supported?
            switch (x[0])
            {
                //Upper Case
                case >= (char)ConsoleKey.A and <= (char)ConsoleKey.Z:
                //Symbols
                case '!' or '#' or '$' or '%':
                    shift = true;
                    break;
                case '&' or '(':
                    shift = true;
                    break;
                case '@':
                    shift = true;
                    break;
                case '^':
                    shift = true;
                    break;
                case '*':
                    shift = true;
                    break;
                case ')':
                    shift = true;
                    break;
                case ';':
                    break;
                case ':':
                    shift = true;
                    break;
                case '+':
                    shift = true;
                    break;
                case '<' or '>' or '?':
                    shift = true;
                    break;
                case '_':
                    shift = true;
                    break;
                case '~':
                    shift = true;
                    break;
                //Left({) and Right(}) Curly Bracket and Vertical Line(|)
                case >= (char)(ConsoleKey.Oem4 - 0x60) and <= (char)(ConsoleKey.Oem6 - 0x60):
                    shift = true;
                    break;
                case '"':
                    shift = true;
                    break;
            }

            return new ConsoleKeyInfo(x[0], key, shift, alt, control);
        }*/

        public static int CursorLeft
        {
            get
            {
                IntPtr consoleBuffer = Interop.Kernel32.CreateFile_IntPtr("CONOUT$",
                    Interop.Kernel32.GenericOperations.GENERIC_READ, FileShare.None, FileMode.Open,
                    Interop.Kernel32.FileAttributes.FILE_ATTRIBUTE_NORMAL);

                Interop.Kernel32.GetConsoleScreenBufferInfo(consoleBuffer, out Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

                Interop.Kernel32.CloseHandle(consoleBuffer);

                return bufferInfo.dwCursorPosition.X;
            }
            set
            {
                if (value >= 0)
                {

                    IntPtr consoleBuffer = Interop.Kernel32.CreateFile_IntPtr("CONOUT$",
                        Interop.Kernel32.GenericOperations.GENERIC_WRITE, FileShare.None, FileMode.Open,
                        Interop.Kernel32.FileAttributes.FILE_ATTRIBUTE_NORMAL);

                    Interop.Kernel32.GetConsoleScreenBufferInfo(consoleBuffer, out Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

                    bufferInfo.dwCursorPosition.X = (short)value;
                    Interop.Kernel32.SetConsoleCursorPosition(consoleBuffer, bufferInfo.dwCursorPosition);

                    Interop.Kernel32.CloseHandle(consoleBuffer);
                }
            }
        }

        // From https://github.com/dotnet/runtimelab/blob/108fcdb/src/libraries/System.Console/src/System/ConsolePal.Windows.cs#L751-L809
        public static void Clear()
        {
            Interop.Kernel32.COORD coordScreen = default;
            IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);

            Interop.Kernel32.GetConsoleScreenBufferInfo(consoleHandle, out Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO consoleInfo);
            int conSize = consoleInfo.dwSize.X * consoleInfo.dwSize.Y;

            Interop.Kernel32.FillConsoleOutputCharacter(consoleHandle, ' ', conSize, coordScreen, out _);
            Interop.Kernel32.FillConsoleOutputAttribute(consoleHandle, consoleInfo.wAttributes, conSize, coordScreen, out _);
            Interop.Kernel32.SetConsoleCursorPosition(consoleHandle, coordScreen);
        }

        //TODO Fix
        /*[MethodImpl(MethodImplOptions.NoInlining)]
        public static int Read()
        {
            if (_buffer == null)
            {
                ConsoleReadKey* bufferAlloc = stackalloc ConsoleReadKey[BufferCapacity];
                _buffer = bufferAlloc;
                _bufferIndex = 0;

                IntPtr consoleHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_INPUT_HANDLE);
                char* x = stackalloc char[2];

                //Index is lower to ensure that there is room for both enter chars
                while (_bufferIndex < BufferCapacity - 1 && _buffer[_bufferIndex].Key != '\n')
                {
                    //TODO Check if this is required
                    x[1] = '\0';

                    Interop.Kernel32.ReadConsole(consoleHandle, (byte*)x, 1, out _, IntPtr.Zero);

                    //TODO Merge with switch, previously the things both statements checked for were distinct but there is quite a bit of overlap now
                    if (x[0] != '\b' || (x[0] == '\b' && _bufferIndex != 0))
                    {
                        if (x[0] == '\t')
                        {
                            byte length = (byte)(7 - CursorLeft % 8);
                            _buffer[_bufferIndex++] = new ConsoleReadKey(x[0], length);
                            CursorLeft += length;
                        }
                        else
                        {
                            _buffer[_bufferIndex++] = new ConsoleReadKey(x[0]);
                        }

                        Write(x[0]);
                    }

                    switch (x[0])
                    {
                        case '\b' when _bufferIndex > 0:
                            //TODO Allow backspacing though tab, this should be possible by changing backspace guards to use cursor position instead of buffer position
                            _bufferIndex -= 2;
                            CursorLeft -= _buffer[_bufferIndex].Length;
                            continue;
                        case '\n':
                            _bufferLength = _bufferIndex;
                            _bufferIndex = BufferCapacity;
                            break;
                    }
                }

                _bufferIndex = 0;
            }

            char nextChar = _buffer[_bufferIndex++].Key;

            if (nextChar == '\n' && !readLine)
            {
                for (int i = 0; i < _bufferLength; i++)
                {
                    _buffer[i].Free();
                }
                _buffer = null;
            }

            return nextChar;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string ReadLine()
        {
            if (_buffer == null)
            {
                readLine = true;
                Read();
                readLine = false;
                _bufferIndex--;
            }

            int length = _bufferLength - 2;
            char[] chars = new char[length];
            for (int i = 0; _bufferIndex < length; i++, _bufferIndex++)
            {
                chars[i] = _buffer[_bufferIndex].Key;
            }

            string newString = new(chars, 0, length);

            chars.Free();
            for (int i = 0; i < _bufferLength; i++)
            {
                _buffer[i].Free();
            }
            _buffer = null;

            return newString;
        }*/

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
        public static void Write(char value)
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
        public static void Write(char[] buffer)
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
        public static void Write(char[] buffer, int index, int count)
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
        public static void Write(double value)
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
        public static void Write(float value)
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

        private static int Write(ulong value, int decimalLength)
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
