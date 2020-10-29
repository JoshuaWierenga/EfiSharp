using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct EFI_SIMPLE_TEXT_INPUT_PROTOCOL
    {
        private readonly IntPtr _pad;

        private readonly IntPtr _readKeyStroke;

        public void ReadKeyStroke(EFI_SIMPLE_TEXT_INPUT_PROTOCOL* handle, EFI_INPUT_KEY* key)
        {
            ((delegate*<EFI_SIMPLE_TEXT_INPUT_PROTOCOL*, EFI_INPUT_KEY*, void>)_readKeyStroke)(handle, key);
        }
    }
}
