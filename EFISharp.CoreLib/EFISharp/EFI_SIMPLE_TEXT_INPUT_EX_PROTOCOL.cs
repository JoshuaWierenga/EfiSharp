using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL
    {
        public static EFI_GUID Guid = new EFI_GUID(0xdd9e7534, 0x7762, 0x4698, 0x8c, 0x14, 0xf5, 0x85, 0x17, 0xa6, 0x25, 0xaa);
    }
}
