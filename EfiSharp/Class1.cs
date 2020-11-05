using System;
using System.Runtime;

namespace EfiSharp
{
    public class Class1
    {
        [RuntimeExport("Main")]
        public static void Main()
        {
            ConsoleTest();
        }

        private static void ConsoleMirror()
        {
            while (true)
            {
                System.Console.Write("Input: ");
                System.Console.WriteLine((char)System.Console.Read());
            }
        }

        public static unsafe void ConsoleTest()
        {
            System.Console.WriteLine("string Output Test");

            System.Console.Write('c');
            System.Console.Write('h');
            System.Console.Write('a');
            System.Console.Write('r');
            System.Console.WriteLine(" Output Test");

            /*char[] testArray = { 't', 'e', 's', 't', '\0' };
            Console.Write("char[] Output Test: ");
            //TODO Figure out why this hangs if in a separate function
            fixed (char* pTestArray = &testArray[0])
            {
                SystemTable->ConOut->OutputString(SystemTable->ConOut, pTestArray);
            }*/

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

            System.Console.WriteLine("\nInput Test:");
            //TODO Fix array issues, currently a program with arrays fails link.exe
            char* input = Console.ReadLine();
            System.Console.Write("You entered: ");
            Console.WriteLine(input);

            System.Console.Write("\nClear Screen(yes/no)?: ");
            input = Console.ReadLine();
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

            if (match && input[4] == '\0')
            {
                System.Console.Clear();
                System.Console.WriteLine("Console Clear Test");
            }

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
            System.Console.Write("White, ");
            //Kind of meant to be invisible, this shows that it works and is also useful for the background test
            System.Console.ForegroundColor = ConsoleColor.Black;
            System.Console.WriteLine("Black");

            System.Console.WriteLine("\nBackground Colour Test");
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
    }
}
