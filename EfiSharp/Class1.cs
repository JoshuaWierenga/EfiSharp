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

            System.Console.Write("Clear Screen(yes/no)?: ");
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

            if (!match || input[4] != '\0') return;
            
            System.Console.Clear();
            System.Console.WriteLine("Console Clear Test");
        }
    }
}
