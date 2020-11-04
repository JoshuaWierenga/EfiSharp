using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
    {
        //TODO Replace with function pointers?
        private readonly IntPtr _pad1;
        private readonly IntPtr _outputString;
        private readonly IntPtr _pad2;
        private readonly IntPtr _pad3;
        private readonly IntPtr _pad4;
        private readonly IntPtr _pad5;
        private readonly IntPtr _clearScreen;

        //TODO Support EFI_STATUS?
        public void OutputString(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, char* str)
        {
            ((delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, char*, void>)_outputString)(handle, str);
        }

        public void ClearScreen(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle)
        {
            ((delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, void>)_clearScreen)(handle);
        }
    }
}
