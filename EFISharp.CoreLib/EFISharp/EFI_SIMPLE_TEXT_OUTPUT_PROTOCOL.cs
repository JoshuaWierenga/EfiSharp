using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
    {
        private readonly IntPtr _pad;

        private readonly IntPtr _outputString;
        public void OutputString(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, char* str)
        {
            ((delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, char*, void>)_outputString)(handle, str);
        }
    }
}
