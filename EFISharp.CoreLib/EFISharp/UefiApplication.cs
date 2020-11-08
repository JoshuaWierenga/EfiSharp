using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using EFISharp;

public static unsafe class UefiApplication
{
    public static EFI_SYSTEM_TABLE* SystemTable { get; private set; }

    internal static EFI_HANDLE ImageHandle;

    [MethodImpl(MethodImplOptions.InternalCall)]
    [RuntimeImport("Main")]
    private static extern void Main();

    [RuntimeExport("EfiMain")]
    private static long EfiMain(EFI_HANDLE imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        ImageHandle = imageHandle;
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

    public static EFI_STATUS CheckExtendedConsoleInput()
    {
        void* ignore;
        //Using EFI_OPEN_PROTOCOL_BY_HANDLE_PROTOCOL as the attribute to indicate that OpenProtocol should do the same thing as HandleProtocol
        //and check if the console in handle supports the extended input protocol but not actually open the protocol. This should always work since
        //the uefi standard requires the console in handle to support both the standard and extended input protocols.
        return UefiApplication.SystemTable->BootServices->OpenProtocol(UefiApplication.SystemTable->ConsoleInHandle,
            EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL.Guid, &ignore, UefiApplication.ImageHandle, EFI_HANDLE.NullHandle,
            EFI_OPEN_PROTOCOL.BY_HANDLE_PROTOCOL);
    }

    //At least when I control the implementation it makes sense to just
    //return a char since that what efi returns, System.Console.Read() does
    //however return an int.
    private static char Read()
    {
        EFI_INPUT_KEY key;
        uint ignore;

        UefiApplication.SystemTable->BootServices->WaitForEvent(1, &In->_waitForKey, &ignore);
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

            //TODO Only allow specific chars, U+0020 to U+007E? https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block) is officially supported by efi
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

    private static void WriteLine()
    {
        char* pValue = stackalloc char[3];
        pValue[0] = '\r';
        pValue[1] = '\n';
        pValue[2] = '\0';

        Out->OutputString(Out, pValue);
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
}
