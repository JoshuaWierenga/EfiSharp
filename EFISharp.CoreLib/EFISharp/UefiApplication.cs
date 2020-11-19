using System.Runtime;
using System.Runtime.CompilerServices;

namespace EfiSharp
{
    public static unsafe class UefiApplication
    {
        public static EFI_SYSTEM_TABLE* SystemTable { get; private set; }
        internal static EFI_HANDLE ImageHandle { get; private set; }

        private static EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* _in;
        public static EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* In => _in;
        //TODO Allow printing to standard error
        public static EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* Out { get; } = SystemTable->ConOut;

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport("Main")]
        private static extern void Main();

        [RuntimeExport("EfiMain")]
        private static long EfiMain(EFI_HANDLE imageHandle, EFI_SYSTEM_TABLE* systemTable)
        {
            ImageHandle = imageHandle;
            SystemTable = systemTable;
            //Console Setup
            SetupExtendedConsoleinput(out _in);

            Main();

            while (true) ;
        }

        private static EFI_STATUS SetupExtendedConsoleinput(out EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* protocol)
        {
            EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* newProtocol;
            EFI_STATUS result = SystemTable->BootServices->OpenProtocol(
                SystemTable->ConsoleInHandle, EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL.Guid, (void**) &newProtocol,
                ImageHandle, EFI_HANDLE.NullHandle, EFI_OPEN_PROTOCOL.GET_PROTOCOL);

            protocol = newProtocol;
            return result;
        }
    }
}