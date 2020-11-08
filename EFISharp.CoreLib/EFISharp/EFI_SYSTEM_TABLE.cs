using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SYSTEM_TABLE
    {
        private readonly EFI_TABLE_HEADER Hdr;
        private readonly char* FirmwareVendor;
        private readonly uint FirmwareRevision;
        internal readonly EFI_HANDLE ConsoleInHandle;
        public readonly EFI_SIMPLE_TEXT_INPUT_PROTOCOL* ConIn;
        private readonly EFI_HANDLE ConsoleOutHandle;
        public readonly EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* ConOut;
        private readonly void* _pad1;
        private readonly void* _pad2;
        private readonly void* _pad3;
        public readonly EFI_BOOT_SERVICES* BootServices;
    }
}
