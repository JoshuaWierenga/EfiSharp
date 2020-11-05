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
        private readonly IntPtr _setAttribute;
        private readonly IntPtr _clearScreen;

        //TODO Support EFI_STATUS?
        public void OutputString(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, char* str)
        {
            ((delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, char*, void>)_outputString)(handle, str);
        }

        //Attribute is processed as two nibbles, the lower nibble is for the text/foreground colour and can be any
        //colour in ConsoleColor, however some of the names are different in the uefi spec. The upper nibble is
        //for the background colour and can only be between 0 and 7, i.e. the first 8 colours in ConsoleColor.
        public void SetAttribute(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, uint attribute)
        {
            ((delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, uint, void>)_setAttribute)(handle, attribute);
        }

        public void ClearScreen(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle)
        {
            ((delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, void>)_clearScreen)(handle);
        }
    }
}
