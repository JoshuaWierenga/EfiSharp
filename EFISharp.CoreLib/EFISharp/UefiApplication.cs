using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using EFISharp;

public static unsafe class UefiApplication
{
    internal static EFI_SYSTEM_TABLE* SystemTable;

    [MethodImpl(MethodImplOptions.InternalCall)]
    [RuntimeImport("Main")]
    public static extern void Main();

    [RuntimeExport("EfiMain")]
    public static long EfiMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        SystemTable = systemTable;
        Console.In = SystemTable->ConIn;
        Console.Out = SystemTable->ConOut;

        Main();

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

        IntPtr* events = stackalloc IntPtr[1];
        events[0] = In->_waitForKey;
        uint ignore;
        
        UefiApplication.SystemTable->BootServices->WaitForEvent(1, events, &ignore);
        In->ReadKeyStroke(In, &key);

        return key.UnicodeChar;
    }

    //TODO Add char*/char[] to string
    public static char* ReadLine()
    {
        char currentKey;
        //TODO Figure out why using array here makes input unstable, screen flashes after every input and after 3-4, the vm crashes entirely
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
