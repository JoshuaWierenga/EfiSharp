using System.Runtime;

namespace EfiSharp
{
    public class Class1
    {
        [RuntimeExport("Main")]
        public static void Main()
        {
            ConsoleMirror();
        }

        private static unsafe void ConsoleMirror()
        {
            while (true)
            {
                Console.Write("Input: ");
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
            Console.Write("\r\nchar* Output Test: ");
            Console.WriteLine(test);
            Console.Write("char* Range Output Test: ");
            Console.WriteLine(test, 1, 2);


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

            Console.Write("\nbool Output Test: ");
            Console.Write(false);
            Console.Write(", ");
            Console.WriteLine(true);

            Console.WriteLine("\nInput Test:");
            //TODO Fix array issues, currently a program with arrays fails link.exe
            char* input = Console.ReadLine();
            Console.Write("You entered: ");
            Console.WriteLine(input);
        }
    }
}
