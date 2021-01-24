using System.Runtime.CompilerServices;
using EfiSharp;

namespace System
{
    //TODO Add beep, https://github.com/fpmurphy/UEFI-Utilities-2019/blob/master/MyApps/Beep/Beep.c
    public static unsafe class Console
    {
        //Queue, Circular Deque?
        //TODO Move to separate class, this requires fixing new
        private static char* _inputBuffer;
        private static int _inputBufferFront;
        private static int _inputBufferRear = -1;
        private const int InputBufferMax = 4096;

        //These colours are used by efi at boot up without prompting the user and so are used here just to match
        private const ConsoleColor DefaultBackgroundColour = ConsoleColor.Black;
        private const ConsoleColor DefaultForegroundColour = ConsoleColor.Gray;

        public static bool KeyAvailable => _inputBufferFront != _inputBufferRear + 1 && _inputBufferRear != InputBufferMax - 1;

        public static ConsoleKeyInfo ReadKey()
        {
            return ReadKey(false);
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            EFI_KEY_DATA input;
            uint ignore;

            UefiApplication.SystemTable->BootServices->WaitForEvent(1,
                &UefiApplication.In->_waitForKeyEx, &ignore);
            UefiApplication.In->ReadKeyStrokeEx(&input);

            if (!intercept)
            {
                Write(input.Key.UnicodeChar);
            }

            if (input.Key.UnicodeChar >= 127)
            {
                return new ConsoleKeyInfo(input.Key.UnicodeChar, 0, false, false, false);
            }

            //Table used to convert between Unicode Basic Latin characters and those in ConsoleKey, https://unicode-table.com/en/blocks/basic-latin/
            //TODO Support other unicode blocks or other chars supported by uefi?
            //I tried different ways of having this defined statically like using stackalloc or a static constructor but everything
            //i tried causes compiler errors, crashes at runtime when accessing it or 0 always being returned.
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
                0, //61
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

            ConsoleKey key = keyMap[input.Key.UnicodeChar];
            bool shift = (input.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_SHIFT_PRESSED) != 0 ||
                         (input.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_SHIFT_PRESSED) != 0;
            bool alt = (input.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_ALT_PRESSED) != 0 ||
                       (input.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_ALT_PRESSED) != 0;
            bool control = (input.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_CONTROL_PRESSED) != 0 ||
                           (input.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_CONTROL_PRESSED) != 0;

            //TODO Replace with Array of ConsoleKeys where the index corresponds with the unicode char
            //and the value is the corresponding ConsoleKey. The conversion is too complex for the switch
            //statement given here. Also need to figure out how to deal with different layouts, is it even 
            //possible if keys outside Basic Latin are used. Are the supplements supported?
            switch (input.Key.UnicodeChar)
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

            return new ConsoleKeyInfo(input.Key.UnicodeChar, key, shift, alt, control);
        }

        //TODO Check if this is possible on efi
        /*public static int CursorSize
        {
            [UnsupportedOSPlatform("browser")]
            get { return ConsolePal.CursorSize; }
            [SupportedOSPlatform("windows")]
            set { ConsolePal.CursorSize = value; }
        }*/

        //[SupportedOSPlatform("windows")]
        public static bool NumberLock
        {
            get
            {
                EFI_KEY_DATA key = new EFI_KEY_DATA();
                return UefiApplication.In->ReadKeyStrokeEx(&key) ==
                       EFI_STATUS.EFI_SUCCESS &&
                       (key.KeyState.KeyToggleState & EFI_KEY_TOGGLE_STATE.EFI_NUM_LOCK_ACTIVE) != 0;
            }
        }

        //[SupportedOSPlatform("windows")]
        public static bool CapsLock
        {
            get
            {
                EFI_KEY_DATA key = new EFI_KEY_DATA();
                return UefiApplication.In->ReadKeyStrokeEx(&key) ==
                       EFI_STATUS.EFI_SUCCESS &&
                       (key.KeyState.KeyToggleState & EFI_KEY_TOGGLE_STATE.EFI_CAPS_LOCK_ACTIVE) != 0;
            }
        }

        //[UnsupportedOSPlatform("browser")]
        public static ConsoleColor BackgroundColor
        {
            get => (ConsoleColor)((byte)UefiApplication.Out->Mode->Attribute >> 4);
            set
            {
                //Only lower nibble colours are supported by efi
                if ((uint)value >= 8) return;
                UefiApplication.Out->SetAttribute(((nuint)value << 4) + (uint)ForegroundColor);
            }
        }

        //[UnsupportedOSPlatform("browser")]
        public static ConsoleColor ForegroundColor
        {
            get => (ConsoleColor)(UefiApplication.Out->Mode->Attribute & 0b1111);
            set => UefiApplication.Out->SetAttribute(((nuint)BackgroundColor << 4) + (uint)value);
        }

        public static int BufferWidth
        {
            //[UnsupportedOSPlatform("browser")]
            get
            {
                nuint width, height;
                UefiApplication.Out->QueryMode((nuint)UefiApplication.Out->Mode->Mode, &width, &height);
                return (int)width;
            }
            //[SupportedOSPlatform("windows")]
            //set { }
        }

        public static int BufferHeight
        {
            //[UnsupportedOSPlatform("browser")]
            get
            {
                nuint width, height;
                UefiApplication.Out->QueryMode((nuint)UefiApplication.Out->Mode->Mode, &width, &height);
                return (int)height;
            }
            //[SupportedOSPlatform("windows")]
            //set { }
        }

        //[UnsupportedOSPlatform("browser")]
        public static void ResetColor()
        {
            UefiApplication.Out->SetAttribute(((nuint)DefaultBackgroundColour << 4) + (nuint)DefaultForegroundColour);
        }

        public static bool CursorVisible
        {
            //[SupportedOSPlatform("windows")]
            get => UefiApplication.Out->Mode->CursorVisible;
            //[UnsupportedOSPlatform("browser")]
            set => UefiApplication.Out->EnableCursor(value);
        }

        //TODO Enforce maximum, EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL.QueryMode(...)
        //[UnsupportedOSPlatform("browser")]
        public static int CursorLeft
        {
            get => UefiApplication.Out->Mode->CursorColumn;
            set
            {
                if (value >= 0)
                {
                    UefiApplication.Out->SetCursorPosition((nuint)value, (nuint)CursorTop);
                }
            }
        }

        //TODO Enforce maximum, EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL.QueryMode(...)
        //[UnsupportedOSPlatform("browser")]
        public static int CursorTop
        {
            get => UefiApplication.Out->Mode->CursorRow;
            set
            {
                if (value >= 0)
                {
                    UefiApplication.Out->SetCursorPosition((nuint)CursorLeft, (nuint)value);
                }
            }
        }

        //TODO Add ValueTuple?
        /// <summary>Gets the position of the cursor.</summary>
        /// <returns>The column and row position of the cursor.</returns>
        /// <remarks>
        /// Columns are numbered from left to right starting at 0. Rows are numbered from top to bottom starting at 0.
        /// </remarks>
        //[UnsupportedOSPlatform("browser")]
        /*public static (int Left, int Top) GetCursorPosition()
        {
            return (CursorLeft, CursorTop);
        }*/

        public static void Clear()
        {
            UefiApplication.Out->ClearScreen();
        }

        //[UnsupportedOSPlatform("browser")]
        //TODO Enforce maximum, EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL.QueryMode(...)
        public static void SetCursorPosition(int left, int top)
        {
            if (left >= 0 && top >= 0)
            {
                UefiApplication.Out->SetCursorPosition((nuint)left, (nuint)top);
            }
        }

        //
        // Give a hint to the code generator to not inline the common console methods. The console methods are
        // not performance critical. It is unnecessary code bloat to have them inlined.
        //
        // Moreover, simple repros for codegen bugs are often console-based. It is tedious to manually filter out
        // the inlined console writelines from them.
        //
        [MethodImpl(MethodImplOptions.NoInlining)]
        //[UnsupportedOSPlatform("browser")]
        //TODO Check if EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL.RegisterKeyNotify() can be used instead of the queue
        //TODO handle control chars
        public static int Read()
        {
            if (_inputBuffer == null)
            {
                char* newBuffer = stackalloc char[InputBufferMax];
                _inputBuffer = newBuffer;
            }

            if (!KeyAvailable)
            {
                EFI_KEY_DATA input;
                uint ignore;

                do
                {
                    UefiApplication.SystemTable->BootServices->WaitForEvent(1,
                        &UefiApplication.In->_waitForKeyEx, &ignore);
                    UefiApplication.In->ReadKeyStrokeEx(&input);

                    if (input.Key.UnicodeChar != (char)ConsoleKey.Enter)
                    {
                        Write(input.Key.UnicodeChar);
                        //Backspace needs to get this far since we need Write(backspace) to visually remove a key from the screen
                        if (input.Key.UnicodeChar == (char)ConsoleKey.Backspace && _inputBufferRear != _inputBufferFront)
                        {
                            //TODO Rewrite to follow queue design or use a different array structure
                            _inputBufferRear--;
                        }
                        else if (_inputBufferRear != InputBufferMax - 1)
                        {
                            _inputBuffer[++_inputBufferRear] = input.Key.UnicodeChar;
                        }

                    }
                } while (input.Key.UnicodeChar != (char)ConsoleKey.Enter);

                WriteLine();
            }

            return KeyAvailable ? _inputBuffer[_inputBufferFront++] : '\0';
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        //[UnsupportedOSPlatform("browser")]
        public static string ReadLine()
        {
            Read();
            int remainingCharCount = _inputBufferRear - _inputBufferFront + 1;

            //TODO Add char.ToString
            if (remainingCharCount <= 0) return new string(_inputBuffer, _inputBufferFront - 1, 1);

            //To simplify this call, the char returned by Read is ignored and retrieved again by accessing the buffer one char earlier
            string newString = new string(_inputBuffer, _inputBufferFront - 1, remainingCharCount + 1);
            _inputBufferFront += remainingCharCount;
            return newString;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine()
        {
            //TODO Make line terminator changeable
            char* pValue = stackalloc char[3];
            pValue[0] = '\r';
            pValue[1] = '\n';
            pValue[2] = '\0';

            UefiApplication.Out->OutputString(pValue);
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

        //TODO Add nullable?
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

        //TODO Add single and double Write
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(decimal value) { }

        //TODO Add decimal type
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(double value) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(float value) { }*/

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }

        //TODO Add CLSCompliantAttribute?
        //[CLSCompliant(false)]
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

        //TODO Add CLSCompliantAttribute?
        //[CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }

        //TODO Add .ToString(), Nullable?
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(object? value) { } */

        [MethodImpl(MethodImplOptions.NoInlining)]
        //TODO Add Nullable?
        public static void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        //TODO Add format string
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(string format, object? arg0) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(string format, object? arg0, object? arg1) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(string format, object? arg0, object? arg1, object? arg2) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(string format, params object?[]? arg)
        {
            if (arg == null)                       // avoid ArgumentNullException from String.Format
                - // faster than Out.WriteLine(format, (Object)arg);
            else
                -
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(string format, object? arg0) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(string format, object? arg0, object? arg1) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(string format, object? arg0, object? arg1, object? arg2) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(string format, params object?[]? arg)
        {
            if (arg == null)                   // avoid ArgumentNullException from String.Format
                - // faster than Out.Write(format, (Object)arg);
            else
                -
        }*/

        //TODO make char arrays constant, use string instead?
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(bool value)
        {
            if (value)
            {
                char* pValue = stackalloc char[5];
                pValue[0] = 'T';
                pValue[1] = 'r';
                pValue[2] = 'u';
                pValue[3] = 'e';
                pValue[4] = '\0';

                UefiApplication.Out->OutputString(pValue);
            }
            else
            {
                char* pValue = stackalloc char[6];
                pValue[0] = 'F';
                pValue[1] = 'a';
                pValue[2] = 'l';
                pValue[3] = 's';
                pValue[4] = 'e';
                pValue[5] = '\0';

                UefiApplication.Out->OutputString(pValue);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(char value)
        {
            char* pValue = stackalloc char[2];
            pValue[0] = value;
            pValue[1] = '\0';

            UefiApplication.Out->OutputString(pValue);
        }

        //Todo Add nullable?
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(char[] buffer)
        {
            if (buffer == null) return;

            fixed (char* pBuffer = buffer)
            {
                UefiApplication.Out->OutputString(pBuffer);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(char[] buffer, int index, int count)
        {
            int maxIndex = index + count;
            if (buffer == null || index >= count || maxIndex > buffer.Length) return;

            char* pBuffer = stackalloc char[count + 1];
            for (int i = 0; i < count; i++)
            {
                pBuffer[i] = buffer[index + i];
            }
            pBuffer[count] = '\0';
            
            UefiApplication.Out->OutputString(pBuffer);
        }

        //TODO Add single and double Write
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(double value) { }

        //TODO Add decimal Type
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(decimal value) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(float value) { }*/

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(int value)
        {
            //This is needed to prevent value overflowing for -value being >int.MaxValue, I tried simply adding Write((uint)(-value), 1)); but that fails for all negative numbers.
            uint unsignedValue = (uint)value;

            if (value < 0)
            {
                Write('-');
                //TODO Add Math.Abs?
                unsignedValue = (uint)(-value);
            }

            Write(unsignedValue, 10);
        }

        //TODO Add CLSCompliantAttribute?
        //[CLSCompliant(false)]
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
                //TODO Add Math.Abs?
                value = -value;
            }

            Write((ulong)value, 20);
        }

        //TODO Add CLSCompliantAttribute? 
        //[CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(ulong value)
        {
            Write(value, 20);
        }

        private static void Write(ulong value, int decimalLength)
        {
            char* pValue = stackalloc char[decimalLength + 1];
            sbyte digitPosition = (sbyte)(decimalLength - 1); //This is designed to wrap around for numbers with decimalLength digits

            do
            {
                pValue[digitPosition--] = (char)(value % 10 + '0');
                value /= 10;
            } while (value > 0);

            UefiApplication.Out->OutputString(&pValue[digitPosition + 1]);
        }

        //TODO Add .ToString(), Nullable?
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(object? value) { }*/

        [MethodImpl(MethodImplOptions.NoInlining)]
        //TODO Add Nullable?
        public static void Write(string value)
        {
            fixed (char* pValue = value)
            {
                UefiApplication.Out->OutputString(pValue);
            }
        }
    }
}