using System;
using System.Runtime;
using EFISharp;

namespace EfiSharp
{
    public class Class1
    {
        [RuntimeExport("Main")]
        public static void Main()
        {
            ConsoleSize();
            ConsoleTest();
        }

        private static unsafe void ConsoleSize()
        {
            System.Console.Write("Current Mode: ");
            System.Console.WriteLine(UefiApplication.SystemTable->ConOut->Mode->Mode);
            System.Console.Write("Current Size: ");
            System.Console.Write('(');
            System.Console.Write(System.Console.BufferWidth);
            System.Console.Write(", ");
            System.Console.Write(System.Console.BufferHeight);
            System.Console.WriteLine(")");


            ulong modeCount = (ulong)UefiApplication.SystemTable->ConOut->Mode->MaxMode;
            ulong cols = 0, rows = 0;

            System.Console.Write("Supported modes: ");
            for (ulong i = 0; i < modeCount; i++)
            {
                UefiApplication.SystemTable->ConOut->QueryMode(UefiApplication.SystemTable->ConOut, i, &cols, &rows);

                System.Console.Write("\r\nMode ");
                System.Console.Write(i);
                System.Console.Write(" Size: ");
                System.Console.Write('(');
                System.Console.Write(cols);
                System.Console.Write(", ");
                System.Console.Write(rows);
                System.Console.Write(")");
            }

            ulong selectedMode = 0;
            bool invalidInput = true;
            while (invalidInput)
            {
                System.Console.Write("\r\nSelect Mode: ");
                selectedMode = (ulong)System.Console.Read() - 0x30;
                if (selectedMode < modeCount)
                {
                    invalidInput = false;
                }
            }

            UefiApplication.SystemTable->ConOut->SetMode(UefiApplication.SystemTable->ConOut, selectedMode);
            System.Console.Write("\r\nNew Mode: ");
            System.Console.WriteLine(UefiApplication.SystemTable->ConOut->Mode->Mode);

            System.Console.WriteLine("Press key to continue");
            System.Console.Read();
            System.Console.Clear();
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
            //ConsoleKeyTest();
            ConsoleInputTest();
            ConsoleInputExTest();
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
            //TODO Figure out why this hangs if in a separate function
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

        private static unsafe void ConsoleClearTest()
        {
            System.Console.Write("\nClear Screen(yes/no)?: ");
            char* input = Console.ReadLine();
            bool match = true;
            fixed (char* yes = "yes")
            {
                //TODO Use EFI_UNICODE_COLLATION_PROTOCOL
                int i = 0;
                while (match && i < 3)
                {
                    if (input[i] == '\0' || input[i] != yes[i])
                    {
                        match = false;
                    }
                    i++;
                }
            }

            if (!match || input[4] != '\0') return;
            System.Console.Clear();
            System.Console.WriteLine("Console Clear Test");
        }

        private static unsafe void ConsoleInputTest()
        {
            System.Console.WriteLine("\nInput Test:");
            //TODO Fix array issues, currently a program with arrays fails link.exe
            char* input = Console.ReadLine();
            System.Console.Write("You entered: ");
            Console.WriteLine(input);
        }

        public static unsafe void ConsoleInputExTest()
        {
            /*System.Console.WriteLine("\r\nConsole Extended Input Protocol Existence test");

            EFI_STATUS result = Console.CheckExtendedConsoleInput();
            switch (result)
            {
                case EFI_STATUS.EFI_SUCCESS:
                    System.Console.WriteLine("Success");
                    break;
                default:
                    System.Console.WriteLine();
                    System.Console.Write((ulong)result);
                    break;
            }*/

            System.Console.WriteLine("\r\nConsole Extended Input Protocol Setup test");
            EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* input = Console.SetupExtendedConsoleinput();
            if (input != null)
            {
                System.Console.WriteLine("Success");
            }

            System.Console.WriteLine("\r\nConsole Extended Input Protocol test");
            System.Console.WriteLine("Enter any key and optionally use modifier and toggle keys, e.g. ctrl, alt and caps lock:");

            EFI_KEY_DATA key;
            uint ignore;
            UefiApplication.SystemTable->BootServices->WaitForEvent(1, &input->_waitForKeyEx, &ignore);
            ulong result = (ulong) input->ReadKeyStrokeEx(input, &key);
            //System.Console.Write("Status: ");
            //System.Console.WriteLine(result);

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
            if ((key.KeyState.KeyToggleState & KeyToggleState.EFI_TOGGLE_STATE_VALID) != 0)
            {
                if ((key.KeyState.KeyToggleState & KeyToggleState.EFI_SCROLL_LOCK_ACTIVE) != 0)
                {
                    System.Console.Write(" Scroll Lock");
                }
                if ((key.KeyState.KeyToggleState & KeyToggleState.EFI_NUM_LOCK_ACTIVE) != 0)
                {
                    System.Console.Write(" Num Lock");
                }
                if ((key.KeyState.KeyToggleState & KeyToggleState.EFI_CAPS_LOCK_ACTIVE) != 0)
                {
                    System.Console.Write(" Caps Lock");
                }
                System.Console.WriteLine();
            }
            else
            {
                System.Console.WriteLine("Fail");
            }
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

            char input;
            while (true)
            {
                input = (char)System.Console.Read();
                switch (input)
                {
                    case 'w':
                        System.Console.CursorTop--;
                        break;
                    case 'a':
                        System.Console.CursorLeft--;
                        break;
                    case 's':
                        System.Console.CursorTop++;
                        break;
                    case 'd':
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

                switch ((char)System.Console.Read())
                {
                    case 'w':
                        System.Console.CursorTop--;
                        break;
                    case 'a':
                        System.Console.CursorLeft--;
                        break;
                    case 's':
                        System.Console.CursorTop++;
                        break;
                    case 'd':
                        System.Console.CursorLeft++;
                        break;
                }
            }
        }
    }
}
