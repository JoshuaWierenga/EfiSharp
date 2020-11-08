using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SYSTEM_TABLE
    {
        private readonly EFI_TABLE_HEADER Hdr;
        private readonly char* FirmwareVendor;
        private readonly uint FirmwareRevision;
        //TODO Add EFI_HANDLE
        //TODO Make internal and setup EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL in UefiApplication
        public readonly void* ConsoleInHandle;
        public readonly EFI_SIMPLE_TEXT_INPUT_PROTOCOL* ConIn;
        private readonly void* ConsoleOutHandle;
        public readonly EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* ConOut;
        private readonly void* _pad1;
        private readonly void* _pad2;
        private readonly void* _pad3;
        public readonly EFI_BOOT_SERVICES* BootServices;
    }
}
