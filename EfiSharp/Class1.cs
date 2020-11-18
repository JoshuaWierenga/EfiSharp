using System;
using System.Runtime;

namespace EfiSharp
{
    public class Class1
    {
        [RuntimeExport("Main")]
        public static void Main()
        {
            ConsoleSize();
            //ConsoleMirror();
            ConsoleTest();
        }

        private static unsafe void ConsoleSize()
        {
            System.Console.Write("Current Mode: ");
            System.Console.WriteLine(Console.Out->Mode->Mode);
            System.Console.Write("Current Size: ");
            System.Console.Write('(');
            System.Console.Write(System.Console.BufferWidth);
            System.Console.Write(", ");
            System.Console.Write(System.Console.BufferHeight);
            System.Console.WriteLine(")");


            uint modeCount = (uint)Console.Out->Mode->MaxMode;
            nuint cols = 0, rows = 0;

            System.Console.Write("Supported modes: ");
            for (uint i = 0; i < modeCount; i++)
            {
                Console.Out->QueryMode(i, &cols, &rows);

                System.Console.Write("\r\nMode ");
                System.Console.Write(i);
                System.Console.Write(" Size: ");
                System.Console.Write('(');
                System.Console.Write(cols);
                System.Console.Write(", ");
                System.Console.Write(rows);
                System.Console.Write(")");
            }

            nuint selectedMode = 0;
            bool invalidInput = true;
            while (invalidInput)
            {
                System.Console.Write("\r\nSelect Mode: ");
                selectedMode = (nuint)System.Console.ReadKey().KeyChar - 0x30;
                if (selectedMode < modeCount)
                {
                    invalidInput = false;
                }
            }

            Console.Out->SetMode(selectedMode);
            System.Console.Write("\r\nNew Mode: ");
            System.Console.WriteLine(Console.Out->Mode->Mode);

            System.Console.WriteLine("Press any key to continue");
            System.Console.ReadKey();
            System.Console.Clear();
        }

        private static void ConsoleReadMirror()
        {
            while (true)
            {
                System.Console.Write("Input: ");
                int input = System.Console.Read();
                if (input is not '\0' or 0xD)
                {
                    System.Console.Write("\r\nReceived: ");
                    System.Console.WriteLine((char)input);
                }
            }
        }

        private static void ConsoleMirror()
        {
            while (true)
            {
                System.Console.Write("Input: ");
                System.Console.WriteLine((char)System.Console.Read());
            }
        }

        public static void ConsoleTest()
        {
            ConsolePrimitiveTests();
            ConsoleInputTest();
            ConsoleInputExTest();
            ConsoleKeyTest();
            ConsoleClearTest();
            ConsoleColourTest();
            ConsoleSizeTest();
            ExtendedConsoleCursorTest();
        }

        private static unsafe void ConsolePrimitiveTests()
        {
            System.Console.WriteLine("string Output Test");

            System.Console.Write('c');
            System.Console.Write('h');
            System.Console.Write('a');
            System.Console.Write('r');
            System.Console.WriteLine(" Output Test");

            char[] testArray = { 't', 'e', 's', 't', '\0' };
            System.Console.Write("char[] Output Test: ");
            //TODO Add char array printing function
            fixed (char* pTestArray = &testArray[0])
            {
                Console.Write(pTestArray);
            }

            char* test = stackalloc char[5];
            test[0] = 't';
            test[1] = 'e';
            test[2] = 's';
            test[3] = 't';
            test[4] = '\0';
            System.Console.Write("\r\nchar* Output Test: ");
            Console.WriteLine(test);
            System.Console.Write("char* Range Output Test: ");
            Console.WriteLine(test, 1, 2);

            System.Console.WriteLine("New Line Output Test");
            System.Console.WriteLine();

            System.Console.Write("sbyte Output Test: Minimum: ");
            System.Console.Write(sbyte.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(sbyte.MaxValue);

            System.Console.Write("short Output Test: Minimum: ");
            System.Console.Write(short.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(short.MaxValue);

            System.Console.Write("int Output Test: Minimum: ");
            System.Console.Write(int.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(int.MaxValue);

            System.Console.Write("long Output Test: Minimum: ");
            System.Console.Write(long.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(long.MaxValue);

            System.Console.Write("\nbyte Output Test: Minimum: ");
            System.Console.Write(byte.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(byte.MaxValue);

            System.Console.Write("ushort Output Test: Minimum: ");
            System.Console.Write(ushort.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(ushort.MaxValue);

            System.Console.Write("uint Output Test: Minimum: ");
            System.Console.Write(uint.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(uint.MaxValue);

            System.Console.Write("ulong Output Test: Minimum: ");
            System.Console.Write(ulong.MinValue);
            System.Console.Write(", Maximum: ");
            System.Console.WriteLine(ulong.MaxValue);

            System.Console.Write("\nbool Output Test: ");
            System.Console.Write(false);
            System.Console.Write(", ");
            System.Console.WriteLine(true);
        }

        private static void ConsoleInputTest()
        {
            System.Console.Write("\r\nReadLine Input Test: ");
            string input = System.Console.ReadLine();
            System.Console.Write("You entered: ");
            System.Console.WriteLine(input);
        }

        public static unsafe void ConsoleInputExTest()
        {
            System.Console.WriteLine("\r\nExtended Input Protocol test");
            System.Console.WriteLine("Enter any key and optionally use modifier and toggle keys, e.g. ctrl, alt and caps lock:");

            EFI_KEY_DATA key;
            uint ignore;
            UefiApplication.SystemTable->BootServices->WaitForEvent(1, &Console.In->_waitForKeyEx, &ignore);
            Console.In->ReadKeyStrokeEx(&key);

            System.Console.Write("Key: ");
            System.Console.WriteLine(key.Key.UnicodeChar);

            System.Console.Write("Key Shift Data:");
            if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_SHIFT_STATE_VALID) != 0)
            {
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_SHIFT_PRESSED) != 0)
                {
                    System.Console.Write(" Left Shift");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_CONTROL_PRESSED) != 0)
                {
                    System.Console.Write(" Left Ctrl");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_ALT_PRESSED) != 0)
                {
                    System.Console.Write(" Left Alt");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_LOGO_PRESSED) != 0)
                {
                    System.Console.Write(" Left Win");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_SHIFT_PRESSED) != 0)
                {
                    System.Console.Write(" Right Shift");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_CONTROL_PRESSED) != 0)
                {
                    System.Console.Write(" Right Ctrl");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_ALT_PRESSED) != 0)
                {
                    System.Console.Write(" Right Alt");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_LOGO_PRESSED) != 0)
                {
                    System.Console.Write(" Right Win");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_MENU_KEY_PRESSED) != 0)
                {
                    System.Console.Write(" Menu Key");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_SYS_REQ_PRESSED) != 0)
                {
                    System.Console.Write(" Sys Req Key");
                }
                System.Console.WriteLine();
            }
            else
            {
                System.Console.WriteLine(" Fail");
            }

            System.Console.Write("Key Toggle Data:");
            if ((key.KeyState.KeyToggleState & EFI_KEY_TOGGLE_STATE.EFI_TOGGLE_STATE_VALID) != 0)
            {
                if ((key.KeyState.KeyToggleState & EFI_KEY_TOGGLE_STATE.EFI_SCROLL_LOCK_ACTIVE) != 0)
                {
                    System.Console.Write(" Scroll Lock");
                }
                //This technically could be wrong since it recalls ReadKeyStrokeEx but it is very soon after the last call and so should match
                if (System.Console.NumberLock)
                {
                    System.Console.Write(" Num Lock");
                }
                if (System.Console.CapsLock)
                {
                    System.Console.Write(" Caps Lock");
                }
                System.Console.WriteLine();
            }
            else
            {
                System.Console.WriteLine(" Fail");
            }
        }

        private static void ConsoleKeyTest()
        {
            System.Console.WriteLine("\r\nReadKey Input Test");
            System.Console.Write("Enter any key and optionally use modifier keys, i.e. shift, ctrl and alt: ");
            ConsoleKeyInfo keyInfo = System.Console.ReadKey();

            System.Console.WriteLine();
            switch (keyInfo.Key)
            {
                case >= ConsoleKey.A and <= ConsoleKey.Z:
                    System.Console.WriteLine("You entered a letter");
                    break;
                case >= ConsoleKey.D0 and <= ConsoleKey.D9 when (keyInfo.Modifiers & ConsoleModifiers.Shift) == 0:
                    System.Console.WriteLine("You entered a number");
                    break;
                case ConsoleKey.Backspace:
                    System.Console.WriteLine("You pressed backspace");
                    break;
                case ConsoleKey.Tab:
                    System.Console.WriteLine("You pressed tab");
                    break;
                case ConsoleKey.Enter:
                    System.Console.WriteLine("You pressed enter");
                    break;
                case ConsoleKey.Spacebar:
                    System.Console.WriteLine("You pressed space");
                    break;
                case >= ConsoleKey.D0 and <= ConsoleKey.D9: //shift + digit symbols
                case ConsoleKey.Oem1:
                case ConsoleKey.OemPlus:
                case ConsoleKey.OemComma:
                case ConsoleKey.OemMinus:
                case ConsoleKey.OemPeriod:
                case ConsoleKey.Oem2:
                case ConsoleKey.Oem3:
                case ConsoleKey.Oem4:
                case ConsoleKey.Oem5:
                case ConsoleKey.Oem6:
                case ConsoleKey.Oem7:
                    System.Console.WriteLine("You entered a symbol");
                    break;
                default:
                    System.Console.WriteLine("Entered key is not supported");
                    System.Console.WriteLine((int)keyInfo.KeyChar);
                    break;
            }

            if (keyInfo.Modifiers != 0)
            {
                System.Console.Write("You held down:");

                if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    System.Console.Write(" Shift");
                }
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    System.Console.Write(" Ctrl");
                }
                if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0)
                {
                    System.Console.Write(" Alt");
                }
                System.Console.WriteLine();
            }
        }

        private static void ConsoleClearTest()
        {
            System.Console.Write("\nClear Screen(yes/no)?: ");
            string input = System.Console.ReadLine();
            bool match = input == "yes";

            if (!match) return;
            System.Console.Clear();
            System.Console.WriteLine("Console Clear Test");
        }

        private static void ConsoleColourTest()
        {
            System.Console.WriteLine("\r\nForeground Colour Test");
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.Write("Dark Blue, ");
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.Write("Dark Green, ");
            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
            System.Console.Write("Dark Cyan, ");
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.Write("Dark Red, ");
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            System.Console.Write("Dark Magenta, ");
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.Write("Dark Yellow, ");
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine("Gray");
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.Write("Blue, ");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write("Green, ");
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.Write("Cyan, ");
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("Red, ");
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.Write("Magenta, ");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write("Yellow, ");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.Write("Dark Gray, ");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("White");

            System.Console.WriteLine("\nBackground Colour Test and Black Foreground Colour Test");
            System.Console.ForegroundColor = ConsoleColor.Black;
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.Write("Dark Blue, ");
            System.Console.BackgroundColor = ConsoleColor.DarkGreen;
            System.Console.Write("Dark Green, ");
            System.Console.BackgroundColor = ConsoleColor.DarkCyan;
            System.Console.Write("Dark Cyan, ");
            System.Console.BackgroundColor = ConsoleColor.DarkRed;
            System.Console.Write("Dark Red, ");
            System.Console.BackgroundColor = ConsoleColor.DarkMagenta;
            System.Console.Write("Dark Magenta, ");
            System.Console.BackgroundColor = ConsoleColor.DarkYellow;
            System.Console.Write("Dark Yellow, ");
            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.WriteLine("Gray");

            System.Console.Write("\r\nColour");
            System.Console.ResetColor();
            System.Console.WriteLine(" Reset Test");
        }

        private static void ConsoleSizeTest()
        {
            System.Console.Write("\r\nConsole Size: ");
            System.Console.Write('(');
            System.Console.Write(System.Console.BufferWidth);
            System.Console.Write(", ");
            System.Console.Write(System.Console.BufferHeight);
            System.Console.WriteLine(")");
        }

        private static void ConsoleCursorTest()
        {
            System.Console.Write("\r\nCursor Test");
            System.Console.CursorVisible = true;

            while (true)
            {
                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        System.Console.CursorTop--;
                        break;
                    case ConsoleKey.A:
                        System.Console.CursorLeft--;
                        break;
                    case ConsoleKey.S:
                        System.Console.CursorTop++;
                        break;
                    case ConsoleKey.D:
                        System.Console.CursorLeft++;
                        break;
                }
            }
        }

        private static void ExtendedConsoleCursorTest()
        {
            System.Console.WriteLine("\r\nCursor Test");
            System.Console.Write("Position: ");
            System.Console.CursorVisible = true;
            int xText = System.Console.CursorLeft;
            int yText = System.Console.CursorTop;

            int x, y;
            while (true)
            {
                x = System.Console.CursorLeft;
                y = System.Console.CursorTop;

                System.Console.SetCursorPosition(xText, yText);
                System.Console.Write('(');
                System.Console.Write(x);
                System.Console.Write(", ");
                System.Console.Write(y);
                System.Console.Write(")       ");
                System.Console.SetCursorPosition(x, y);

                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        System.Console.CursorTop--;
                        break;
                    case ConsoleKey.A:
                        System.Console.CursorLeft--;
                        break;
                    case ConsoleKey.S:
                        System.Console.CursorTop++;
                        break;
                    case ConsoleKey.D:
                        System.Console.CursorLeft++;
                        break;
                }
            }
        }
    }
}
