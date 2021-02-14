using System.Runtime;
using System.Runtime.CompilerServices;

namespace EfiSharp
{
    public static unsafe class UefiApplication
    {
        public static EFI_SYSTEM_TABLE* SystemTable { get; private set; }
        //TODO Make internal
        public static EFI_HANDLE ImageHandle { get; private set; }

        public static EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* In;
        //TODO Allow printing to standard error
        public static EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* Out { get; private set; }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport("Main")]
        private static extern void Main();

        [RuntimeExport("EfiMain")]
        private static long EfiMain(EFI_HANDLE imageHandle, EFI_SYSTEM_TABLE* systemTable)
        {
            ImageHandle = imageHandle;
            SystemTable = systemTable;
            //Prevent system reboot after 5 minutes
            SystemTable->BootServices->SetWatchdogTimer(0, 0);
            //Console Setup
            SetupExtendedConsoleinput(out In);
            Out = SystemTable->ConOut;

            Main();

            //TODO: Close EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL?
            while (true) ;
        }

        private static EFI_STATUS SetupExtendedConsoleinput(out EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* protocol)
        {
            EFI_STATUS status = SystemTable->BootServices->OpenProtocol(SystemTable->ConsoleInHandle,
                EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL.Guid, out void* pProtocol, ImageHandle, EFI_HANDLE.NullHandle,
                EFI_OPEN_PROTOCOL.GET_PROTOCOL);
            protocol = (EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL*)pProtocol;
            return status;
        }
    }
}
