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
            //ConsoleReadLineMirror();
            ConsoleTest();
        }

        //TODO Move to EfiSharp.Console and call on startup from EfiMain, current attempts cause the linker to complain and insist that this project implements it
        private static unsafe void ConsoleSize()
        {
            Console.Write("Current Mode: ");
            Console.WriteLine(UefiApplication.Out->Mode->Mode);
            Console.Write("Current Size: ");
            Console.Write('(');
            Console.Write(Console.BufferWidth);
            Console.Write(", ");
            Console.Write(Console.BufferHeight);
            Console.WriteLine(")");

            uint modeCount = (uint)UefiApplication.Out->Mode->MaxMode;
            nuint cols = 0, rows = 0;

            Console.Write("Supported modes: ");
            for (uint i = 0; i < modeCount; i++)
            {
                UefiApplication.Out->QueryMode(i, &cols, &rows);

                Console.Write("\r\nMode ");
                Console.Write(i);
                Console.Write(" Size: ");
                Console.Write('(');
                Console.Write(cols);
                Console.Write(", ");
                Console.Write(rows);
                Console.Write(")");
            }

            nuint selectedMode = 0;
            while (true)
            {
                Console.Write("\r\nSelect Mode: ");
                selectedMode = (nuint)Console.ReadKey().KeyChar - 0x30;
                if (selectedMode < modeCount)
                {
                    break;
                }
            }

            UefiApplication.Out->SetMode(selectedMode);
            Console.Write("\r\nNew Mode: ");
            Console.WriteLine(UefiApplication.Out->Mode->Mode);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        private static void ConsoleReadMirror()
        {
            while (true)
            {
                Console.Write("Input: ");
                int input = Console.Read();
                if (input is not '\0' or 0xD)
                {
                    Console.Write("\r\nReceived: ");
                    Console.WriteLine((char)input);
                }
            }
        }

        private static void ConsoleReadLineMirror()
        {
            while (true)
            {
                Console.Write("Input: ");
                Console.WriteLine(Console.ReadLine());
            }
        }

        public static void ConsoleTest()
        {
            ConsolePrimitiveTests();
            ConsoleFloatingPointTests();
            ConsoleRandomTest();
            ConsoleInputTest();
            ConsoleInputExTest();
            ConsoleKeyTest();
            ConsoleClearTest();
            ConsoleColourTest();
            ConsoleSizeTest();
            ExtendedConsoleCursorTest();
        }

        private static void ConsolePrimitiveTests()
        {
            Console.WriteLine("string Output Test");

            Console.Write('c');
            Console.Write('h');
            Console.Write('a');
            Console.Write('r');
            Console.WriteLine(" Output Test");

            char[] array = { 't', 'e', 's', 't' };
            Console.Write("char[] Output Test: ");
            Console.WriteLine(array);

            Console.Write("char[] Range Output Test: ");
            Console.WriteLine(array, 1, 2);
            array.Dispose();

            Console.WriteLine("New Line Output Test");
            Console.WriteLine();

            Console.Write("sbyte Output Test: Minimum: ");
            Console.Write(sbyte.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(sbyte.MaxValue);

            Console.Write("short Output Test: Minimum: ");
            Console.Write(short.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(short.MaxValue);

            Console.Write("int Output Test: Minimum: ");
            Console.Write(int.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(int.MaxValue);

            Console.Write("long Output Test: Minimum: ");
            Console.Write(long.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(long.MaxValue);

            Console.Write("\nbyte Output Test: Minimum: ");
            Console.Write(byte.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(byte.MaxValue);

            Console.Write("ushort Output Test: Minimum: ");
            Console.Write(ushort.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(ushort.MaxValue);

            Console.Write("uint Output Test: Minimum: ");
            Console.Write(uint.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(uint.MaxValue);

            Console.Write("ulong Output Test: Minimum: ");
            Console.Write(ulong.MinValue);
            Console.Write(", Maximum: ");
            Console.WriteLine(ulong.MaxValue);

            /*Console.Write("float Output Test: Test 1: ");
            //Console.Write(-3.40282347E+38f);
            Console.Write(3.14159E+4f);
            //Console.Write(3.40282347E+38);
            Console.Write(", Test 2: ");
            Console.Write(-9.999999f);
            Console.Write(", Test 3: ");
            Console.WriteLine(3.1f);*/

            Console.Write("\nbool Output Test: ");
            Console.Write(false);
            Console.Write(", ");
            Console.WriteLine(true);
        }

        private static void ConsoleFloatingPointTests()
        {
            Console.WriteLine();
            Console.WriteLine("float Output Test:       | double Output Test:");
            Console.WriteLine("Actual     | Converted   | Actual               | Converted");

            //TODO Add string.PadRight
            Console.Write("31415.9    | ");
            Console.Write(3.14159E+4f);
            Console.Write("  | 31415.9              | ");
            Console.WriteLine(3.14159E+4d);

            Console.Write("-9.999999  | ");
            Console.Write(-9.999999f);
            Console.Write(" | -9.9999999999999     | ");
            Console.WriteLine(-9.9999999999999d);

            Console.Write("6.3        | ");
            Console.Write(6.3f);
            Console.Write("  | 6.3                  | ");
            Console.WriteLine(6.3d);

            Console.Write("3.14159265 | ");
            Console.Write(3.14159265f);
            Console.Write("  | 3.141592653589793238 | ");
            Console.WriteLine(3.141592653589793238d);

            Console.Write("177.004    | ");
            Console.Write(177.004f);
            Console.Write("  | 177.0000000004       | ");
            Console.WriteLine(177.0000000004d);

            Console.Write("-14999.004 | ");
            Console.Write(-14999.004f);
            Console.Write(" | -14999.000000004444  | ");
            Console.WriteLine(-14999.000000004444d);

            Console.Write("-1234.5678 | ");
            Console.Write(-1234.5678f);
            Console.Write(" | -1234.56789101112    | ");
            Console.WriteLine(-1234.56789101112d);

            Console.Write("1.2345678  | ");
            Console.Write(1.2345678f);
            Console.Write("  | 1.23456789101112     | ");
            Console.WriteLine(1.23456789101112d);

            Console.Write("8.765432   | ");
            Console.Write(8.765432f);
            Console.Write("  | 12.111098765432      | ");
            Console.WriteLine(12.111098765432d);

            Console.Write("18.0009    | ");
            Console.Write(18.0009f);
            Console.Write("  | 18.00000000000009    | ");
            Console.WriteLine(18.00000000000009d);

            Console.Write("18.00009   | ");
            Console.Write(18.00009f);
            Console.Write("  | 18.000000000000009   | ");
            Console.WriteLine(18.000000000000009d);

            Console.Write("-141.0001  | ");
            Console.Write(-141.0001f);
            Console.Write(" | -14141.000000000001  | ");
            Console.WriteLine(-14141.000000000001d);
        }

        private static void ConsoleRandomTest()
        {
            Console.WriteLine("\r\nRandom Test");

            Random rng = new();
            byte[] num = new byte[1];
            rng.NextBytes(num);

            Console.Write("EFI Random values: ");
            Console.Write(num[0]);
            Console.Write(", ");
            Console.Write(rng.Next());
            Console.Write(", ");
            Console.Write(rng.Next(50));
            Console.Write(", ");
            Console.Write(rng.Next(-75, 75));
            Console.Write(", ");
            Console.Write(rng.NextInt64());
            Console.Write(", ");
            Console.Write(rng.NextInt64(3*(long)uint.MaxValue));
            Console.Write(", ");
            Console.Write(rng.NextInt64(-4 * uint.MaxValue, 4 * (long)uint.MaxValue));
            Console.Write(", ");
            Console.Write(rng.NextSingle());
            Console.Write(", ");
            Console.Write(rng.NextDouble());
            Console.WriteLine();

            num.Dispose();
            rng.Dispose();

            rng = new Random(1);
            num = new byte[1];
            rng.NextBytes(num);

            //Ensure the seed works on Random.LegacyImpl.cs
            Console.Write("Legacy Static values: ");
            Console.Write(num[0]);
            Console.Write(", ");
            Console.Write(rng.Next());
            Console.Write(", ");
            Console.Write(rng.Next(50));
            Console.Write(", ");
            Console.Write(rng.Next(-75, 75));
            Console.Write(", ");
            Console.Write(rng.NextInt64());
            Console.Write(", ");
            Console.Write(rng.NextInt64(3 * (long)uint.MaxValue));
            Console.Write(", ");
            Console.Write(rng.NextInt64(-4 * uint.MaxValue, 4 * (long)uint.MaxValue));
            Console.Write(", ");
            Console.Write(rng.NextSingle());
            Console.Write(", ");
            Console.Write(rng.NextDouble());
            Console.WriteLine();

            num.Dispose();
            rng.Dispose();
        }

        private static void ConsoleInputTest()
        {
            Console.Write("\r\nReadLine Input Test: ");
            string input = Console.ReadLine();
            Console.Write("You entered: ");
            Console.WriteLine(input);
            input.Dispose();
        }

        public static unsafe void ConsoleInputExTest()
        {
            Console.WriteLine("\r\nExtended Input Protocol test");
            Console.WriteLine("Enter any key and optionally use modifier and toggle keys, e.g. ctrl, alt and caps lock:");

            UefiApplication.SystemTable->BootServices->WaitForEvent(UefiApplication.In->_waitForKeyEx, out _);
            UefiApplication.In->ReadKeyStrokeEx(out EFI_KEY_DATA key);

            Console.Write("Key: ");
            Console.WriteLine(key.Key.UnicodeChar);

            Console.Write("Key Shift Data:");
            if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_SHIFT_STATE_VALID) != 0)
            {
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_SHIFT_PRESSED) != 0)
                {
                    Console.Write(" Left Shift");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_CONTROL_PRESSED) != 0)
                {
                    Console.Write(" Left Ctrl");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_ALT_PRESSED) != 0)
                {
                    Console.Write(" Left Alt");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_LEFT_LOGO_PRESSED) != 0)
                {
                    Console.Write(" Left Win");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_SHIFT_PRESSED) != 0)
                {
                    Console.Write(" Right Shift");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_CONTROL_PRESSED) != 0)
                {
                    Console.Write(" Right Ctrl");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_ALT_PRESSED) != 0)
                {
                    Console.Write(" Right Alt");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_RIGHT_LOGO_PRESSED) != 0)
                {
                    Console.Write(" Right Win");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_MENU_KEY_PRESSED) != 0)
                {
                    Console.Write(" Menu Key");
                }
                if ((key.KeyState.KeyShiftState & KeyShiftState.EFI_SYS_REQ_PRESSED) != 0)
                {
                    Console.Write(" Sys Req Key");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(" Fail");
            }

            Console.Write("Key Toggle Data:");
            if ((key.KeyState.KeyToggleState & EFI_KEY_TOGGLE_STATE.EFI_TOGGLE_STATE_VALID) != 0)
            {
                if ((key.KeyState.KeyToggleState & EFI_KEY_TOGGLE_STATE.EFI_SCROLL_LOCK_ACTIVE) != 0)
                {
                    Console.Write(" Scroll Lock");
                }
                //This technically could be wrong since it recalls ReadKeyStrokeEx but it is very soon after the last call and so should match
                if (Console.NumberLock)
                {
                    Console.Write(" Num Lock");
                }
                if (Console.CapsLock)
                {
                    Console.Write(" Caps Lock");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(" Fail");
            }
        }

        private static void ConsoleKeyTest()
        {
            Console.WriteLine("\r\nReadKey Input Test");
            Console.Write("Enter any key and optionally use modifier keys, i.e. shift, ctrl and alt: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            Console.WriteLine();
            switch (keyInfo.Key)
            {
                case >= ConsoleKey.A and <= ConsoleKey.Z:
                    Console.WriteLine("You entered a letter");
                    break;
                case >= ConsoleKey.D0 and <= ConsoleKey.D9 when (keyInfo.Modifiers & ConsoleModifiers.Shift) == 0:
                    Console.WriteLine("You entered a number");
                    break;
                case ConsoleKey.Backspace:
                    Console.WriteLine("You pressed backspace");
                    break;
                case ConsoleKey.Tab:
                    Console.WriteLine("You pressed tab");
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine("You pressed enter");
                    break;
                case ConsoleKey.Spacebar:
                    Console.WriteLine("You pressed space");
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
                    Console.WriteLine("You entered a symbol");
                    break;
                default:
                    Console.WriteLine("Entered key is not supported");
                    Console.WriteLine((int)keyInfo.KeyChar);
                    break;
            }

            if (keyInfo.Modifiers != 0)
            {
                Console.Write("You held down:");

                if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    Console.Write(" Shift");
                }
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    Console.Write(" Ctrl");
                }
                if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0)
                {
                    Console.Write(" Alt");
                }
                Console.WriteLine();
            }
        }

        private static void ConsoleClearTest()
        {
            Console.Write("\nClear Screen(yes/no)?: ");
            string input = Console.ReadLine();
            bool match = input == "yes";
            input.Dispose();

            if (!match) return;
            Console.Clear();
            Console.WriteLine("Console Clear Test");
        }

        private static void ConsoleColourTest()
        {
            Console.WriteLine("\r\nForeground Colour Test");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Dark Blue, ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Dark Green, ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Dark Cyan, ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Dark Red, ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Dark Magenta, ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Dark Yellow, ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Gray");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Blue, ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Green, ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Cyan, ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Red, ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Magenta, ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Yellow, ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Dark Gray, ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("White");

            Console.WriteLine("\nBackground Colour Test and Black Foreground Colour Test");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("Dark Blue, ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("Dark Green, ");
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write("Dark Cyan, ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("Dark Red, ");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Dark Magenta, ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("Dark Yellow, ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine("Gray");

            Console.Write("\r\nColour");
            Console.ResetColor();
            Console.WriteLine(" Reset Test");
        }

        private static void ConsoleSizeTest()
        {
            Console.Write("\r\nConsole Size: ");
            Console.Write('(');
            Console.Write(Console.BufferWidth);
            Console.Write(", ");
            Console.Write(Console.BufferHeight);
            Console.WriteLine(")");
        }

        private static void ConsoleCursorTest()
        {
            Console.Write("\r\nCursor Test");
            Console.CursorVisible = true;

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        Console.CursorTop--;
                        break;
                    case ConsoleKey.A:
                        Console.CursorLeft--;
                        break;
                    case ConsoleKey.S:
                        Console.CursorTop++;
                        break;
                    case ConsoleKey.D:
                        Console.CursorLeft++;
                        break;
                }
            }
        }

        private static void ExtendedConsoleCursorTest()
        {
            Console.WriteLine("\r\nCursor Test");
            Console.Write("Position: ");
            Console.CursorVisible = true;
            int xText = Console.CursorLeft;
            int yText = Console.CursorTop;

            int x, y;
            while (true)
            {
                x = Console.CursorLeft;
                y = Console.CursorTop;

                Console.SetCursorPosition(xText, yText);
                Console.Write('(');
                Console.Write(x);
                Console.Write(", ");
                Console.Write(y);
                Console.Write(")       ");
                Console.SetCursorPosition(x, y);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        Console.CursorTop--;
                        break;
                    case ConsoleKey.A:
                        Console.CursorLeft--;
                        break;
                    case ConsoleKey.S:
                        Console.CursorTop++;
                        break;
                    case ConsoleKey.D:
                        Console.CursorLeft++;
                        break;
                }
            }
        }
    }
}
