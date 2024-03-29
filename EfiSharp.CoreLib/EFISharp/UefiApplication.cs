using System.Runtime;
using System.Runtime.CompilerServices;

namespace EfiSharp
{
    public static unsafe class UefiApplication
    {
        public static EFI_SYSTEM_TABLE* SystemTable { get; private set; }
        internal static EFI_HANDLE ImageHandle { get; private set; }

        public static EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* In;
        //TODO Allow printing to standard error
        public static EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* Out { get; private set; }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport("__managed__Main")]
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
            Internal.Console.ConsoleSetup();

            Main();
            while (true) ;
        }

        //Note that there is no need to close protocols opened with EFI_OPEN_PROTOCOL.Get_Protocol
        private static EFI_STATUS SetupExtendedConsoleinput(out EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* protocol)
        {
            EFI_STATUS status = SystemTable->BootServices->OpenProtocol(SystemTable->ConsoleInHandle,
                EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL.EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL_GUID, out void* pProtocol,
                ImageHandle, EFI_HANDLE.NullHandle, EFI_OPEN_PROTOCOL.GET_PROTOCOL);
            protocol = (EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL*)pProtocol;
            return status;
        }
    }
}
