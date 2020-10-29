using System;
using System.Runtime;
using EFISharp;

public static unsafe class UefiApplication
{
    private static EFI_SYSTEM_TABLE* SystemTable;

    //TODO Remove function, so far this is required for exe outputtype,
    //dropping that prevents link for finding EfiMain
    public static void Main()
    {

    }

    //TODO Find way to call into other projects that this library does not depend on,
    //this would allow matching c#'s regular main method and allow booting a library
    //without it having a version of this function, extern?
    [RuntimeExport("EfiMain")]
    public static long EfiMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        SystemTable = systemTable;
        Console.In = SystemTable->ConIn;
        Console.Out = SystemTable->ConOut;

        Console.WriteLine("string Output Test");

        Console.Write('c');
        Console.Write('h');
        Console.Write('a');
        Console.Write('r');
        Console.WriteLine(" Output Test");

        char* test = stackalloc char[5];
        test[0] = 't';
        test[1] = 'e';
        test[2] = 's';
        test[3] = 't';
        test[4] = '\0';
        Console.Write("char* Output Test: ");
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
        for (int i = 0; input[i] != '\0'; i++)
        {
            Console.Write(input[i]);
        }

        while (true) ;
    }
}

public static unsafe class Console
{
    internal const int ReadBufferSize = 4096;

    internal static EFI_SIMPLE_TEXT_INPUT_PROTOCOL* In;
    internal static EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* Out;

    //At least when I control the implementation it makes sense to just
    //return a char since that what efi returns, System.Console.Read() does
    //however return an int.
    public static char Read()
    {
        EFI_INPUT_KEY key;

        //TODO Add WaitForEvent/WaitForKey
        do
        {
            In->ReadKeyStroke(In, &key);
        } while (key.UnicodeChar == default); //TODO Use ScanCode

        return key.UnicodeChar;
    }

    //TODO Add char*/char[] to string
    public static char* ReadLine()
    {
        char currentKey;
        char* input = stackalloc char[ReadBufferSize];
        int charCount = 0;

        do
        {
            currentKey = Read();
            if (currentKey == '\r')
            {
                break;
            }

            //TODO Only allow specific chars, U+0020 to U+007E? https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block)
            input[charCount] = currentKey;
            charCount++;
        } while (charCount < ReadBufferSize);

        char* result = stackalloc char[charCount + 1];
        for (int i = 0; i < charCount; i++)
        {
            result[i] = input[i];
        }
        result[charCount] = '\0';

        return result;
    }

    public static void WriteLine()
    {
        char* pValue = stackalloc char[3];
        pValue[0] = '\r';
        pValue[1] = '\n';
        pValue[2] = '\0';

        Out->OutputString(Out, pValue);
    }

    public static void WriteLine(bool value)
    {
        Write(value);
        WriteLine();
    }

    public static void WriteLine(char value)
    {
        Write(value);
        WriteLine();
    }

    public static void WriteLine(char* buffer)
    {
        Write(buffer);
        WriteLine();
    }

    public static void WriteLine(char* buffer, int index, int count)
    {
        Write(buffer, index, count);
        WriteLine();
    }

    //TODO Add single and double WriteLine
    /*public static void WriteLine(decimal value)
    {

    }

    public static void WriteLine(double value)
    {

    }

    public static void WriteLine(float value)
    {

    }*/

    public static void WriteLine(int value)
    {
        Write(value);
        WriteLine();
    }

    public static void WriteLine(uint value)
    {
        Write(value);
        WriteLine();
    }

    public static void WriteLine(long value)
    {
        Write(value);
        WriteLine();
    }

    public static void WriteLine(ulong value)
    {
        Write(value);
        WriteLine();
    }

    /*public static void WriteLine(object? value)
    {
        Out.WriteLine(value);
    }*/

    public static void WriteLine(string value)
    {
        Write(value);
        WriteLine();
    }

    /*public static void WriteLine(string format, object? arg0)
    {
    }

    public static void WriteLine(string format, object? arg0, object? arg1)
    {
    }

    public static void WriteLine(string format, object? arg0, object? arg1, object? arg2)
    {
    }

    public static void WriteLine(string format, params object?[]? arg)
    {
    }

    public static void Write(string format, object? arg0)
    {
    }

    public static void Write(string format, object? arg0, object? arg1)
    {
    }

    public static void Write(string format, object? arg0, object? arg1, object? arg2)
    {
    }

    public static void Write(string format, params object?[]? arg)
    {
    }*/

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

            Out->OutputString(Out, pValue);
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

            Out->OutputString(Out, pValue);
        }
    }

    public static void Write(char value)
    {
        char* pValue = stackalloc char[2];
        pValue[0] = value;
        pValue[1] = '\0';

        Out->OutputString(Out, pValue);
    }

    public static void Write(char* buffer)
    {
        Out->OutputString(Out, buffer);
    }

    public static void Write(char* buffer, int index, int count)
    {
        char* pBuffer = stackalloc char[count + 1];

        for (int i = 0, j = index; i < count && buffer[j] != '\0'; i++, j++)
        {
            pBuffer[i] = buffer[j];
        }

        pBuffer[count] = '\0';
        Out->OutputString(Out, pBuffer);
    }

    //TODO Add single and double Write
    /*public static void Write(decimal value)
    {
    }

    public static void Write(double value)
    {
    }

    public static void Write(float value)
    {
    }*/

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

    //TODO Rewrite to make a single pointer array for char of max uint length and use a single loop
    public static void Write(uint value)
    {
        //TODO Figure out why new fails at runtime
        byte* digits = stackalloc byte[10];
        byte digitCount = 0;
        byte digitPosition = 9; //This is designed to wrap round for numbers with 10 digits

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

        Out->OutputString(Out, pValue);
    }

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

    //TODO Rewrite to make a single pointer array for char of max ulong length and use a single loop
    public static void Write(ulong value)
    {
        //TODO Figure out why new fails at runtime
        byte* digits = stackalloc byte[19];
        byte digitCount = 0;
        byte digitPosition = 18; //This is designed to wrap round for numbers with 19 digits

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

        Out->OutputString(Out, pValue);
    }

    /*public static void Write(object? value)
    {
    }*/

    public static void Write(string value)
    {
        fixed (char* pValue = value)
        {
            Out->OutputString(Out, pValue);
        }
    }
}
