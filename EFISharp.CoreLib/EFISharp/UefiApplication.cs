using System;
using System.Runtime;
using EFISharp;

public static unsafe class EfiApplication
{
    private static EFI_SYSTEM_TABLE* SystemTable;

    //TODO Remove function, so far this is required for exe outputtype,
    //dropping that prevents link for finding EfiMain
    public static void Main()
    {

    }

    //TODO Find way to call into other projects that this library does not depend on,
    //this would allow matching c#'s regular main method and allow booting a library
    //without it having a version of this function
    [RuntimeExport("EfiMain")]
    public static long EfiMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        SystemTable = systemTable;

        WriteLine("Hello world!");
        WriteLine(sbyte.MinValue);
        WriteLine(sbyte.MaxValue);
        WriteLine(short.MinValue);
        WriteLine(short.MaxValue);
        WriteLine(int.MinValue);
        WriteLine(int.MaxValue);
        WriteLine(long.MinValue);
        WriteLine(long.MaxValue);
        Write('\n');
        WriteLine(byte.MinValue);
        WriteLine(byte.MaxValue);
        WriteLine(ushort.MinValue);
        WriteLine(ushort.MaxValue);
        WriteLine(uint.MinValue);
        WriteLine(uint.MaxValue);
        WriteLine(ulong.MinValue);
        WriteLine(ulong.MaxValue);

        while (true) ;
    }

    public static void WriteLine(string value)
    {
        Write(value);
        //TODO Add String.Concat?
        Write("\r\n");
    }

    public static void WriteLine(long value)
    {
        Write(value);
        //TODO Add String.Concat?
        Write("\r\n");
    }

    public static void WriteLine(ulong value)
    {
        Write(value);
        //TODO Add String.Concat?
        Write("\r\n");
    }

    public static void WriteLine(char value)
    {
        Write(value);
        //TODO Add String.Concat?
        Write("\r\n");
    }

    //TODO Remove, add console classes?
    public static void Write(string value)
    {
        fixed (char* pValue = value)
        {
            SystemTable->ConOut->OutputString(SystemTable->ConOut, pValue);
        }
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

    //TODO Rewrite to make a single pointer array for char of max long length and use a single loop
    public static void Write(ulong value)
    {
        //TODO Figure out why new fails at runtime
        byte* digits = stackalloc byte[19];
        byte digitCount = 0;
        byte digitPosition = 18; //This is designed to wrap round for numbers with 19 digits

        //From https://stackoverflow.com/a/4808815
        do
        {
            digits[digitPosition] = (byte) (value % 10);
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

        SystemTable->ConOut->OutputString(SystemTable->ConOut, pValue);
    }

    public static void Write(char value)
    {
        char* pValue = stackalloc char[2];
        pValue[0] = value;
        pValue[1] = '\0';

        SystemTable->ConOut->OutputString(SystemTable->ConOut, pValue);
    }
}
