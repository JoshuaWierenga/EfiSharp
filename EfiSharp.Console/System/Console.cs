using System.Runtime.CompilerServices;
using EFISharp;

namespace System
{
    //TODO Add Console.ReadKey
    //TODO Add Window Info, SIMPLE_TEXT_OUTPUT_PROTOCOL.GetMode(...)
    //TODO Support modifier keys, SIMPLE_TEXT_INPUT_EX_PROTOCOL
    public unsafe class Console
    {
        //These colours are used by efi at boot up without prompting the user and so are used here just to match
        private const ConsoleColor DefaultBackgroundColour = ConsoleColor.Black;
        private const ConsoleColor DefaultForegroundColour = ConsoleColor.Gray;
        
        //TODO Use SIMPLE_TEXT_OUTPUT_MODE.Attribute?
        private static ConsoleColor _backgroundColor = DefaultBackgroundColour;
        private static ConsoleColor _foregroundColor = DefaultForegroundColour;

        //TODO Check if this is possible on efi
        /*public static int CursorSize
        {
            [UnsupportedOSPlatform("browser")]
            get { return ConsolePal.CursorSize; }
            [SupportedOSPlatform("windows")]
            set { ConsolePal.CursorSize = value; }
        }*/

        //[UnsupportedOSPlatform("browser")]
        public static ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                //Only lower nibble colours are supported by efi
                if ((uint)value >= 8) return;
                _backgroundColor = value;
                UefiApplication.SystemTable->ConOut->SetAttribute(UefiApplication.SystemTable->ConOut, ((uint)value << 4) + (uint)_foregroundColor);
            }
        }

        //[UnsupportedOSPlatform("browser")]
        public static ConsoleColor ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                UefiApplication.SystemTable->ConOut->SetAttribute(UefiApplication.SystemTable->ConOut, ((uint)_backgroundColor << 4) + (uint)value);
            }
        }

        //[UnsupportedOSPlatform("browser")]
        public static void ResetColor()
        {
            _backgroundColor = DefaultBackgroundColour;
            _foregroundColor = DefaultForegroundColour;
            UefiApplication.SystemTable->ConOut->SetAttribute(UefiApplication.SystemTable->ConOut, ((uint)_backgroundColor << 4) + (uint)_foregroundColor);
        }

        public static bool CursorVisible
        {
            //[SupportedOSPlatform("windows")]
            get => UefiApplication.SystemTable->ConOut->Mode->CursorVisible;
            //[UnsupportedOSPlatform("browser")]
            set => UefiApplication.SystemTable->ConOut->EnableCursor(UefiApplication.SystemTable->ConOut, value);
        }

        //TODO Enforce maximum, EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL.QueryMode(...)
        //[UnsupportedOSPlatform("browser")]
        public static int CursorLeft
        {
            //TODO Fix get cursor column
            get => UefiApplication.SystemTable->ConOut->Mode->CursorColumn;
            set
            {
                if (value >= 0)
                {
                    UefiApplication.SystemTable->ConOut->SetCursorPosition(UefiApplication.SystemTable->ConOut, (uint)value, (uint)CursorTop);
                }
            }
        }

        //TODO Enforce maximum, EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL.QueryMode(...)
        //[UnsupportedOSPlatform("browser")]
        public static int CursorTop
        {
            get => UefiApplication.SystemTable->ConOut->Mode->CursorRow;
            set
            {
                if (value >= 0)
                {
                    UefiApplication.SystemTable->ConOut->SetCursorPosition(UefiApplication.SystemTable->ConOut, (uint)CursorLeft, (uint)value);
                }
            }
        }

        //TODO Add ValueTuple?
        /// <summary>Gets the position of the cursor.</summary>
        /// <returns>The column and row position of the cursor.</returns>
        /// <remarks>
        /// Columns are numbered from left to right starting at 0. Rows are numbered from top to bottom starting at 0.
        /// </remarks>
        /*[UnsupportedOSPlatform("browser")]
        public static (int Left, int Top) GetCursorPosition()
        {
            return ConsolePal.GetCursorPosition();
        }*/

        public static void Clear()
        {
            UefiApplication.SystemTable->ConOut->ClearScreen(UefiApplication.SystemTable->ConOut);
        }

        //[UnsupportedOSPlatform("browser")]
        //TODO Enforce maximum, EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL.QueryMode(...)
        public static void SetCursorPosition(int left, int top)
        {
            if (left >= 0 && top >= 0)
            {
                UefiApplication.SystemTable->ConOut->SetCursorPosition(UefiApplication.SystemTable->ConOut, (uint)left, (uint)top);
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
        //TODO handle control chars, enter, backspace, ...
        public static int Read()
        {
            EFI_INPUT_KEY key;
            uint ignore;

            UefiApplication.SystemTable->BootServices->WaitForEvent(1, &UefiApplication.SystemTable->ConIn->_waitForKey, &ignore);
            UefiApplication.SystemTable->ConIn->ReadKeyStroke(UefiApplication.SystemTable->ConIn, &key);

            return key.UnicodeChar;
        }

        /*[MethodImpl(MethodImplOptions.NoInlining)]
        //[UnsupportedOSPlatform("browser")]
        public static string ReadLine() { }*/

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine()
        {
            //TODO Make line terminator changeable
            char* pValue = stackalloc char[3];
            pValue[0] = '\r';
            pValue[1] = '\n';
            pValue[2] = '\0';

            UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
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

        //TODO Add char[]
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(char[]? buffer) { }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void WriteLine(char[] buffer, int index, int count) { }*/

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

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
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

                UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
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

                UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(char value)
        {
            char* pValue = stackalloc char[2];
            pValue[0] = value;
            pValue[1] = '\0';

            UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
        }

        //TODO Fix char[]
        /*[MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(char[]? buffer)
        {
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(char[] buffer, int index, int count)
        {
        }*/

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
            if (value < 0)
            {
                Write('-');
                //TODO Add Math.Abs?
                value = -value;
            }

            Write((uint)value);
        }

        //TODO Add CLSCompliantAttribute?
        //[CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        //TODO Rewrite to make a single pointer array for char of max uint length and use a single loop
        //TODO Add single integer to string function with variable int size
        public static void Write(uint value)
        {
            //TODO Figure out why using array here makes the vm crash on startup
            byte* digits = stackalloc byte[10];
            byte digitCount = 0;
            byte digitPosition = 9; //This is designed to wrap around for numbers with 10 digits

            //From https://stackoverflow.com/a/4808815
            do
            {
                digits[digitPosition] = (byte)(value % 10);
                value = value / 10;
                digitCount++;
                digitPosition--;
            } while (value > 0);

            byte charCount = (byte)(digitCount + 1);

            char* pValue = stackalloc char[charCount];
            pValue[charCount - 1] = '\0';

            digitPosition++;
            for (int i = 0; i < digitCount; i++, digitPosition++)
            {
                pValue[i] = (char)(digits[digitPosition] + '0');
            }

            UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
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

            Write((ulong)value);
        }

        //TODO Add CLSCompliantAttribute? 
        //[CLSCompliant(false)]
        //TODO Rewrite to make a single pointer array for char of max ulong length and use a single loop
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(ulong value)
        {
            //TODO Figure out why using array here makes the vm crash on startup
            byte* digits = stackalloc byte[19];
            byte digitCount = 0;
            byte digitPosition = 18; //This is designed to wrap around for numbers with 19 digits

            //From https://stackoverflow.com/a/4808815
            do
            {
                digits[digitPosition] = (byte)(value % 10);
                value = value / 10;
                digitCount++;
                digitPosition--;
            } while (value > 0);


            byte charCount = (byte)(digitCount + 1);

            char* pValue = stackalloc char[charCount];
            pValue[charCount - 1] = '\0';

            digitPosition++;
            for (int i = 0; i < digitCount; i++, digitPosition++)
            {
                pValue[i] = (char)(digits[digitPosition] + '0');
            }

            UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
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
                UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
            }
        }
    }
}