using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL
    {
        private readonly IntPtr _pad;
        private readonly IntPtr _readKeyStrokeEx;
        public readonly IntPtr _waitForKeyEx;
        //TODO figure out why using this crashes the program on boot
        //readonly IntPtr _setState;

        public EFI_STATUS ReadKeyStrokeEx(EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* handle, EFI_KEY_DATA* key)
        {
            return (EFI_STATUS)((delegate*<EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL*, EFI_KEY_DATA*, ulong>)_readKeyStrokeEx)(handle, key);
        }

        public static readonly EFI_GUID Guid = new EFI_GUID(0xdd9e7534, 0x7762, 0x4698, 0x8c, 0x14, 0xf5, 0x85, 0x17, 0xa6, 0x25, 0xaa);
    }
}
