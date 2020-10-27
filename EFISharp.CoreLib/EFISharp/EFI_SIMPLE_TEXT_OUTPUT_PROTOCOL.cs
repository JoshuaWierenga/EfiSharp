using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
    {
        private readonly IntPtr _pad;

        private readonly IntPtr _outputString;
        public void OutputString(void* handle, char* str)
        {
            ((delegate*<byte*, char*, void>)_outputString)((byte*)handle, str);
        }
    }
}
