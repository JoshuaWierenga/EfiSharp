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

        private static unsafe void ConsoleMirror()
        {
            while (true)
            {
                System.Console.Write("Input: ");
                Console.WriteLine(Console.Read());
            }
        }

        public static unsafe void ConsoleTest()
        {
            Console.WriteLine("string Output Test");

            Console.Write('c');
            Console.Write('h');
            Console.Write('a');
            Console.Write('r');
            Console.WriteLine(" Output Test");

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


            Console.WriteLine("New Line Output Test");
            Console.WriteLine();

            System.Console.Write("sbyte Output Test: Minimum: ");
            Console.Write(sbyte.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(sbyte.MaxValue);

            System.Console.Write("short Output Test: Minimum: ");
            Console.Write(short.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(short.MaxValue);

            System.Console.Write("int Output Test: Minimum: ");
            Console.Write(int.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(int.MaxValue);

            System.Console.Write("long Output Test: Minimum: ");
            Console.Write(long.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(long.MaxValue);

            System.Console.Write("\nbyte Output Test: Minimum: ");
            Console.Write(byte.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(byte.MaxValue);

            System.Console.Write("ushort Output Test: Minimum: ");
            Console.Write(ushort.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(ushort.MaxValue);

            System.Console.Write("uint Output Test: Minimum: ");
            Console.Write(uint.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(uint.MaxValue);

            System.Console.Write("ulong Output Test: Minimum: ");
            Console.Write(ulong.MinValue);
            System.Console.Write(", Maximum: ");
            Console.WriteLine(ulong.MaxValue);

            System.Console.Write("\nbool Output Test: ");
            Console.Write(false);
            System.Console.Write(", ");
            Console.WriteLine(true);

            Console.WriteLine("\nInput Test:");
            //TODO Fix array issues, currently a program with arrays fails link.exe
            char* input = Console.ReadLine();
            System.Console.Write("You entered: ");
            Console.WriteLine(input);
        }
    }
}
