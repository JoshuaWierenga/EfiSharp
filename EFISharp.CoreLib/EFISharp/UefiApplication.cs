using System.Runtime;
using System.Runtime.CompilerServices;

namespace EfiSharp
{
    public static unsafe class UefiApplication
    {
        public static EFI_SYSTEM_TABLE* SystemTable { get; private set; }
        internal static EFI_HANDLE ImageHandle { get; private set; }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport("Main")]
        private static extern void Main();

        [RuntimeExport("EfiMain")]
        private static long EfiMain(EFI_HANDLE imageHandle, EFI_SYSTEM_TABLE* systemTable)
        {
            ImageHandle = imageHandle;
            SystemTable = systemTable;
            //Console Setup
            Console.SetupExtendedConsoleinput(out Console.In);
            Console.Out = SystemTable->ConOut;

            Main();

            while (true) ;
        }
    }

    //TODO Move to namespace to make System.Console easier to use
    public static unsafe class Console
    {
        private const int ReadBufferSize = 4096;

        public static EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* In;
        public static EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* Out;

        internal static EFI_STATUS SetupExtendedConsoleinput(out EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* protocol)
        {
            EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* newProtocol;
            EFI_STATUS result = UefiApplication.SystemTable->BootServices->OpenProtocol(
                UefiApplication.SystemTable->ConsoleInHandle,
                EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL.Guid, (void**)&newProtocol, UefiApplication.ImageHandle,
                EFI_HANDLE.NullHandle,
                EFI_OPEN_PROTOCOL.GET_PROTOCOL);

            protocol = newProtocol;
            return result;
        }

        //At least when I control the implementation it makes sense to just
        //return a char since that what efi returns, System.Console.Read() does
        //however return an int.
        private static char Read()
        {
            EFI_KEY_DATA key;
            uint ignore;

            UefiApplication.SystemTable->BootServices->WaitForEvent(1, &In->_waitForKeyEx, &ignore);
            In->ReadKeyStrokeEx(&key);

            return key.Key.UnicodeChar;
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

            Out->OutputString(pValue);
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
            Out->OutputString(buffer);
        }

        public static void Write(char* buffer, int index, int count)
        {
            char* pBuffer = stackalloc char[count + 1];

            for (int i = 0, j = index; i < count && buffer[j] != '\0'; i++, j++)
            {
                pBuffer[i] = buffer[j];
            }

            pBuffer[count] = '\0';
            Out->OutputString(pBuffer);
        }
    }
}