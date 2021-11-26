// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.IO;
using System.Runtime.CompilerServices;

namespace System
{
    public static unsafe class Console
    {
        private const int BufferCapacity = 512;
        // From https://github.com/dotnet/runtimelab/blob/2b0e278/src/libraries/System.Private.CoreLib/src/System/Text/UTF8Encoding.cs#L780-L798
        private const int ByteBufferCapacity = 3*(BufferCapacity + 1);
        private const int BytesPerWChar = 2;

        // total size is 512 * 2 + 3(512 + 1) = 2.563kb which is almost double the size of the efi version
        //TODO either fix static reference fields or at least try to merge these since ReadConsoleW is just putting utf 16 chars into _byteBuffer
        // currently even that is broken as switching ReadConsoleW to use _charBuffer works but removing the then unused _byteBuffer breaks things, stack deallocation issue?
        private static byte* _byteBuffer;
        private static char* _charBuffer;
        private static int _charPos;
        private static int _charLen;
        private static IntPtr _stdInputHandle;
        private static IntPtr _stdOutputHandle;

        static Console()
        {
            IntPtr consoleBuffer = Interop.Kernel32.CreateFile_IntPtr("CONOUT$",
                Interop.Kernel32.GenericOperations.GENERIC_WRITE, FileShare.None, FileMode.Open,
                Interop.Kernel32.FileAttributes.FILE_ATTRIBUTE_NORMAL);

            // Enable cursor blinking
            Interop.Kernel32.GetConsoleCursorInfo(consoleBuffer, out Interop.Kernel32.CONSOLE_CURSOR_INFO cursorInfo);
            cursorInfo.bVisible = Interop.BOOL.TRUE;
            Interop.Kernel32.SetConsoleCursorInfo(consoleBuffer, ref cursorInfo);

            Interop.Kernel32.CloseHandle(consoleBuffer);
        }

        // ReadLine & Read can't use this because they need to use ReadFile
        // to be able to handle redirected input.  We have to accept that
        // we will lose repeated keystrokes when someone switches from
        // calling ReadKey to calling Read or ReadLine.  Those methods should
        // ideally flush this cache as well.
        private static Interop.InputRecord _cachedInputRecord;

        // Skip non key events. Generally we want to surface only KeyDown event
        // and suppress KeyUp event from the same Key press but there are cases
        // where the assumption of KeyDown-KeyUp pairing for a given key press
        // is invalid. For example in IME Unicode keyboard input, we often see
        // only KeyUp until the key is released.
        private static bool IsKeyDownEvent(Interop.InputRecord ir)
        {
            return (ir.eventType == Interop.KEY_EVENT && ir.keyEvent.keyDown != Interop.BOOL.FALSE);
        }

        private static bool IsModKey(Interop.InputRecord ir)
        {
            // We should also skip over Shift, Control, and Alt, as well as caps lock.
            // Apparently we don't need to check for 0xA0 through 0xA5, which are keys like
            // Left Control & Right Control. See the ConsoleKey enum for these values.
            short keyCode = ir.keyEvent.virtualKeyCode;
            return ((keyCode >= 0x10 && keyCode <= 0x12)
                    || keyCode == 0x14 || keyCode == 0x90 || keyCode == 0x91);
        }

        [Flags]
        internal enum ControlKeyState
        {
            RightAltPressed = 0x0001,
            LeftAltPressed = 0x0002,
            RightCtrlPressed = 0x0004,
            LeftCtrlPressed = 0x0008,
            ShiftPressed = 0x0010,
        }

        // For tracking Alt+NumPad unicode key sequence. When you press Alt key down
        // and press a numpad unicode decimal sequence and then release Alt key, the
        // desired effect is to translate the sequence into one Unicode KeyPress.
        // We need to keep track of the Alt+NumPad sequence and surface the final
        // unicode char alone when the Alt key is released.
        private static bool IsAltKeyDown(Interop.InputRecord ir)
        {
            return (((ControlKeyState)ir.keyEvent.controlKeyState)
                    & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
        }

        private const short AltVKCode = 0x12; // VK_MENU

        public static ConsoleKeyInfo ReadKey()
        {
            return ReadKey(false);
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            // first run setup
            if (_stdInputHandle == default)
            {
                _stdInputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_INPUT_HANDLE);
            }

            Interop.InputRecord ir;
            int numEventsRead = -1;
            bool r;

            if (_cachedInputRecord.eventType == Interop.KEY_EVENT)
            {
                // We had a previous keystroke with repeated characters.
                ir = _cachedInputRecord;
                if (_cachedInputRecord.keyEvent.repeatCount == 0)
                    _cachedInputRecord.eventType = -1;
                else
                {
                    _cachedInputRecord.keyEvent.repeatCount--;
                }
                // We will return one key from this method, so we decrement the
                // repeatCount here, leaving the cachedInputRecord in the "queue".

            }
            else
            { // We did NOT have a previous keystroke with repeated characters:

                while (true)
                {
                    r = Interop.Kernel32.ReadConsoleInput(_stdInputHandle, out ir, 1, out numEventsRead);
                    if (!r || numEventsRead == 0)
                    {
                        // This will fail when stdin is redirected from a file or pipe.
                        // We could theoretically call Console.Read here, but I
                        // think we might do some things incorrectly then.
                        //TODO Add Console SR
                        //throw new InvalidOperationException(SR.InvalidOperation_ConsoleReadKeyOnFile);
                        throw new InvalidOperationException("InvalidOperation_ConsoleReadKeyOnFile");
                    }

                    short keyCode = ir.keyEvent.virtualKeyCode;

                    // First check for non-keyboard events & discard them. Generally we tap into only KeyDown events and ignore the KeyUp events
                    // but it is possible that we are dealing with a Alt+NumPad unicode key sequence, the final unicode char is revealed only when
                    // the Alt key is released (i.e when the sequence is complete). To avoid noise, when the Alt key is down, we should eat up
                    // any intermediate key strokes (from NumPad) that collectively forms the Unicode character.

                    if (!IsKeyDownEvent(ir))
                    {
                        // REVIEW: Unicode IME input comes through as KeyUp event with no accompanying KeyDown.
                        if (keyCode != AltVKCode)
                            continue;
                    }

                    char ch = (char)ir.keyEvent.uChar;

                    // In a Alt+NumPad unicode sequence, when the alt key is released uChar will represent the final unicode character, we need to
                    // surface this. VirtualKeyCode for this event will be Alt from the Alt-Up key event. This is probably not the right code,
                    // especially when we don't expose ConsoleKey.Alt, so this will end up being the hex value (0x12). VK_PACKET comes very
                    // close to being useful and something that we could look into using for this purpose...

                    if (ch == 0)
                    {
                        // Skip mod keys.
                        if (IsModKey(ir))
                            continue;
                    }

                    // When Alt is down, it is possible that we are in the middle of a Alt+NumPad unicode sequence.
                    // Escape any intermediate NumPad keys whether NumLock is on or not (notepad behavior)
                    ConsoleKey key = (ConsoleKey)keyCode;
                    if (IsAltKeyDown(ir) && ((key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9)
                                             || (key == ConsoleKey.Clear) || (key == ConsoleKey.Insert)
                                             || (key >= ConsoleKey.PageUp && key <= ConsoleKey.DownArrow)))
                    {
                        continue;
                    }

                    if (ir.keyEvent.repeatCount > 1)
                    {
                        ir.keyEvent.repeatCount--;
                        _cachedInputRecord = ir;
                    }
                    break;
                }
            } // we did NOT have a previous keystroke with repeated characters.

            ControlKeyState state = (ControlKeyState)ir.keyEvent.controlKeyState;
            bool shift = (state & ControlKeyState.ShiftPressed) != 0;
            bool alt = (state & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
            bool control = (state & (ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed)) != 0;

            ConsoleKeyInfo info = new ConsoleKeyInfo((char)ir.keyEvent.uChar, (ConsoleKey)ir.keyEvent.virtualKeyCode, shift, alt, control);
            if (!intercept)
                Console.Write(ir.keyEvent.uChar);
            return info;
        }

        private static IntPtr InvalidHandleValue => new IntPtr(-1);

        // For ResetColor
        private static volatile bool _haveReadDefaultColors;
        private static volatile byte _defaultColors;

        private static Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO GetBufferInfo()
        {
            bool unused;
            return GetBufferInfo(true, out unused);
        }

        // For apps that don't have a console (like Windows apps), they might
        // run other code that includes color console output.  Allow a mechanism
        // where that code won't throw an exception for simple errors.
        private static Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO GetBufferInfo(bool throwOnNoConsole,
            out bool succeeded)
        {
            succeeded = false;

            IntPtr outputHandle = _stdOutputHandle;
            if (outputHandle == InvalidHandleValue)
            {
                if (throwOnNoConsole)
                {
                    //TODO Add IOException and Console SR
                    //throw new IOException(SR.IO_NoConsole);
                    throw new SystemException("IO_NoConsole");
                }
                return default;
            }

            // Note that if stdout is redirected to a file, the console handle may be a file.
            // First try stdout; if this fails, try stderr and then stdin.
            Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi;
            if (!Interop.Kernel32.GetConsoleScreenBufferInfo(outputHandle, out csbi) &&
                //TODO Add stderr support
                //!Interop.Kernel32.GetConsoleScreenBufferInfo(ErrorHandle, out csbi) &&
                !Interop.Kernel32.GetConsoleScreenBufferInfo(_stdInputHandle, out csbi))
            {
                //TODO Add Marshal.GetLastPInvokeError, Interop.Errors and Win32Marshal.GetExceptionForWin32Error
                /*int errorCode = Marshal.GetLastPInvokeError();
                if (errorCode == Interop.Errors.ERROR_INVALID_HANDLE && !throwOnNoConsole)
                    return default;
                throw Win32Marshal.GetExceptionForWin32Error(errorCode);*/
                throw new SystemException("Win32 Error in Console.GetBufferInfo");
            }

            if (!_haveReadDefaultColors)
            {
                // Fetch the default foreground and background color for the ResetColor method.
                //TODO Add full debug
                //Debug.Assert((int)Interop.Kernel32.Color.ColorMask == 0xff, "Make sure one byte is large enough to store a Console color value!");
                _defaultColors = (byte)(csbi.wAttributes & (short)Interop.Kernel32.Color.ColorMask);
                _haveReadDefaultColors = true; // also used by ResetColor to know when GetBufferInfo has been called successfully
            }

            succeeded = true;
            return csbi;
        }

        private static Interop.Kernel32.Color ConsoleColorToColorAttribute(ConsoleColor color, bool isBackground)
        {
            if ((((int)color) & ~0xf) != 0)
                //TODO Add Console SR
                //throw new ArgumentException(SR.Arg_InvalidConsoleColor);
                throw new ArgumentException("Arg_InvalidConsoleColor");

            Interop.Kernel32.Color c = (Interop.Kernel32.Color)color;

            // Make these background colors instead of foreground
            if (isBackground)
                c = (Interop.Kernel32.Color)((int)c << 4);
            return c;
        }

        private static ConsoleColor ColorAttributeToConsoleColor(Interop.Kernel32.Color c)
        {
            // Turn background colors into foreground colors.
            if ((c & Interop.Kernel32.Color.BackgroundMask) != 0)
            {
                c = (Interop.Kernel32.Color)(((int)c) >> 4);
            }
            return (ConsoleColor)c;
        }

        public static ConsoleColor BackgroundColor
        {
            get
            {
                bool succeeded;
                Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo(false, out succeeded);
                return succeeded ?
                    ColorAttributeToConsoleColor((Interop.Kernel32.Color)csbi.wAttributes & Interop.Kernel32.Color.BackgroundMask) :
                    ConsoleColor.Black; // for code that may be used from Windows app w/ no console
            }
            set
            {
                if (_stdOutputHandle == default)
                {
                    _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
                }

                Interop.Kernel32.Color c = ConsoleColorToColorAttribute(value, true);

                bool succeeded;
                Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo(false, out succeeded);
                // For code that may be used from Windows app w/ no console
                if (!succeeded)
                    return;

                //TODO Add full Debug
                //Debug.Assert(_haveReadDefaultColors, "Setting the background color before we've read the default foreground color!");

                short attrs = csbi.wAttributes;
                attrs &= ~((short)Interop.Kernel32.Color.BackgroundMask);
                // C#'s bitwise-or sign-extends to 32 bits.
                attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)c));
                // Ignore errors here - there are some scenarios for running code that wants
                // to print in colors to the console in a Windows application.
                Interop.Kernel32.SetConsoleTextAttribute(_stdOutputHandle, attrs);
            }
        }

        public static ConsoleColor ForegroundColor
        {
            get
            {
                bool succeeded;
                Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo(false, out succeeded);

                // For code that may be used from Windows app w/ no console
                return succeeded
                    ? ColorAttributeToConsoleColor((Interop.Kernel32.Color)csbi.wAttributes &
                                                   Interop.Kernel32.Color.ForegroundMask)
                    : ConsoleColor.Gray;
            }
            set
            {
                if (_stdOutputHandle == default)
                {
                    _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
                }

                Interop.Kernel32.Color c = ConsoleColorToColorAttribute(value, false);

                bool succeeded;
                Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo(false, out succeeded);
                // For code that may be used from Windows app w/ no console
                if (!succeeded)
                    return;

                //TODO Add full Debug
                //Debug.Assert(_haveReadDefaultColors, "Setting the foreground color before we've read the default foreground color!");

                short attrs = csbi.wAttributes;
                attrs &= ~((short)Interop.Kernel32.Color.ForegroundMask);
                // C#'s bitwise-or sign-extends to 32 bits.
                attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)c));
                // Ignore errors here - there are some scenarios for running code that wants
                // to print in colors to the console in a Windows application.
                Interop.Kernel32.SetConsoleTextAttribute(_stdOutputHandle, attrs);
            }
        }

        public static void ResetColor()
        {
            if (!_haveReadDefaultColors) // avoid the costs of GetBufferInfo if we already know we checked it
            {
                bool succeeded;
                GetBufferInfo(false, out succeeded);
                if (!succeeded)
                    return; // For code that may be used from Windows app w/ no console

                //TODO Add full Debug
                //Debug.Assert(_haveReadDefaultColors, "Resetting color before we've read the default foreground color!");
            }

            if (_stdOutputHandle == default)
            {
                _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
            }

            // Ignore errors here - there are some scenarios for running code that wants
            // to print in colors to the console in a Windows application.
            Interop.Kernel32.SetConsoleTextAttribute(_stdOutputHandle, (short)(ushort)_defaultColors);
        }

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
            if (_stdOutputHandle == default)
            {
                _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
            }

            Interop.Kernel32.COORD coordScreen = default;

            Interop.Kernel32.GetConsoleScreenBufferInfo(_stdOutputHandle, out Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO consoleInfo);
            int conSize = consoleInfo.dwSize.X * consoleInfo.dwSize.Y;

            Interop.Kernel32.FillConsoleOutputCharacter(_stdOutputHandle, ' ', conSize, coordScreen, out _);
            Interop.Kernel32.FillConsoleOutputAttribute(_stdOutputHandle, consoleInfo.wAttributes, conSize, coordScreen, out _);
            Interop.Kernel32.SetConsoleCursorPosition(_stdOutputHandle, coordScreen);
        }

        public static void SetBufferSize(int width, int height)
        {
            // Ensure the new size is not smaller than the console window
            Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo();
            Interop.Kernel32.SMALL_RECT srWindow = csbi.srWindow;
            //TODO Add Console SR
            if (width < srWindow.Right + 1 || width >= short.MaxValue)
                //throw new ArgumentOutOfRangeException(nameof(width), width, SR.ArgumentOutOfRange_ConsoleBufferLessThanWindowSize);
                throw new ArgumentOutOfRangeException("ArgumentOutOfRange_ConsoleBufferLessThanWindowSize");
            if (height < srWindow.Bottom + 1 || height >= short.MaxValue)
                //throw new ArgumentOutOfRangeException(nameof(height), height, SR.ArgumentOutOfRange_ConsoleBufferLessThanWindowSize);
                throw new ArgumentOutOfRangeException("SR.ArgumentOutOfRange_ConsoleBufferLessThanWindowSize");

            Interop.Kernel32.COORD size = default;
            size.X = (short)width;
            size.Y = (short)height;
            if (!Interop.Kernel32.SetConsoleScreenBufferSize(_stdOutputHandle, size))
            {
                //TODO Add Marshal.GetLastPInvokeError and Win32Marshal.GetExceptionForWin32Error
                //throw Win32Marshal.GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
                throw new SystemException("Win32 Error in Console.SetBufferSize");
            }
        }

        public static int BufferWidth
        {
            get
            {
                Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo();
                return csbi.dwSize.X;
            }
            set
            {
                SetBufferSize(value, BufferHeight);
            }
        }

        public static int BufferHeight
        {
            get
            {
                Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO csbi = GetBufferInfo();
                return csbi.dwSize.Y;
            }
            set
            {
                SetBufferSize(BufferWidth, value);
            }
        }


        // From https://github.com/dotnet/runtimelab/blob/2abd487/src/libraries/System.Private.CoreLib/src/System/IO/StreamReader.cs#L595-L664
        // and https://github.com/dotnet/runtimelab/blob/108fcdb/src/libraries/System.Console/src/System/ConsolePal.Windows.cs#L1152-L1185
        private static int ReadBuffer()
        {
            // first run setup
            if (_byteBuffer == default)
            {
                // note that this is slightly risky as stack memory is unallocated automatically, not much choice though
                //TODO Fix static reference fields and then switch to arrays, then just give value at definition
                byte* newByteBuffer = stackalloc byte[ByteBufferCapacity];
                char* newCharBuffer = stackalloc char[BufferCapacity];

                _byteBuffer = newByteBuffer;
                _charBuffer = newCharBuffer;
                _stdInputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_INPUT_HANDLE);
            }

            _charLen = 0;
            _charPos = 0;
            do
            {
                Interop.Kernel32.ReadConsole(_stdInputHandle, _byteBuffer, ByteBufferCapacity / BytesPerWChar,
                    out _charLen, IntPtr.Zero);
                int byteLen = _charLen * BytesPerWChar;

                if (byteLen == 0) // We're at EOF
                {
                    return _charLen;
                }

                // ReadConsoleW gives utf-16 and so no conversion is required
                Buffer.MemoryCopy(_byteBuffer, _charBuffer, BufferCapacity * sizeof(char), byteLen);
            } while (_charLen == 0);
            return _charLen;
        }

        // From https://github.com/dotnet/runtimelab/blob/2abd487/src/libraries/System.Private.CoreLib/src/System/IO/StreamReader.cs#L340-L355
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Read()
        {
            if (_charPos == _charLen)
            {
                if (ReadBuffer() == 0)
                {
                    return -1;
                }
            }
            int result = _charBuffer[_charPos];
            _charPos++;
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string ReadLine()
        {
            if (_charPos == _charLen)
            {
                if (ReadBuffer() == 0)
                {
                    return null;
                }
            }

            string newString = new(_charBuffer, _charPos, _charLen - _charPos - 2);
            _charPos = _charLen;

            return newString;
        }

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
            if (_stdOutputHandle == default)
            {
                _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
            }

            char* pValue = stackalloc char[2];
            pValue[0] = value;
            pValue[1] = '\0';

            int length = 2;
            int bufferSize = 8;

            byte* pBytes = stackalloc byte[bufferSize];
            int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pValue, length,
                pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);

            Interop.Kernel32.WriteFile(_stdOutputHandle, pBytes, cbytes, out _, IntPtr.Zero);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(char[] buffer)
        {
            if (buffer == null) return;

            if (_stdOutputHandle == default)
            {
                _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
            }

            fixed (char* pBuffer = buffer)
            {
                int bufferSize = buffer.Length * 4;

                byte* pBytes = stackalloc byte[bufferSize];
                int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pBuffer,
                    buffer.Length, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);

                Interop.Kernel32.WriteFile(_stdOutputHandle, pBytes, cbytes, out _, IntPtr.Zero);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(char[] buffer, int index, int count)
        {
            if (buffer == null || index >= count || index + count > buffer.Length) return;

            if (_stdOutputHandle == default)
            {
                _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
            }

            fixed (char* pBuffer = &buffer[index])
            {
                int bufferSize = buffer.Length * 4;
                byte* pBytes = stackalloc byte[count];
                int cbytes = Interop.Kernel32.WideCharToMultiByte(Interop.Kernel32.GetConsoleOutputCP(), 0, pBuffer,
                    count, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);

                Interop.Kernel32.WriteFile(_stdOutputHandle, pBytes, cbytes, out _, IntPtr.Zero);
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
                if (_stdOutputHandle == default)
                {
                    _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
                }

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

                Interop.Kernel32.WriteFile(_stdOutputHandle, pBytes, cbytes, out _, IntPtr.Zero);

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
                if (_stdOutputHandle == default)
                {
                    _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
                }

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

                Interop.Kernel32.WriteFile(_stdOutputHandle, pBytes, cbytes, out _, IntPtr.Zero);
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
            if (_stdOutputHandle == default)
            {
                _stdOutputHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE);
            }

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
           
            Interop.Kernel32.WriteFile(_stdOutputHandle, pBytes, cbytes, out _, IntPtr.Zero);

            return length;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Write(string value)
        {
            Internal.Console.Write(value);
        }
    }
}
