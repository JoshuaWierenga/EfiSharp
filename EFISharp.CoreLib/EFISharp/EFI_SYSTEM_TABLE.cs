using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct EFI_SYSTEM_TABLE
    {
        public readonly EFI_TABLE_HEADER Hdr;
        public readonly char* FirmwareVendor;
        public readonly uint FirmwareRevision;
        public readonly void* ConsoleInHandle;
        public readonly void* ConIn;
        public readonly void* ConsoleOutHandle;
        public readonly EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* ConOut;
    }
}
