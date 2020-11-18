using System;
using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
    {
        private readonly IntPtr _pad1;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, char*, void> _outputString;
        private readonly IntPtr _pad2;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, nuint*, nuint*, void> _queryMode;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, void> _setMode;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, void> _setAttribute;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, void> _clearScreen;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, nuint, void> _setCursorPosition;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, bool, void> _enableCursor;
        public readonly SIMPLE_TEXT_OUTPUT_MODE* Mode;

        //Str must be a null terminated string containing only supported characters, typically chars in https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block) and those
        //shown in the related definitions section at https://uefi.org/sites/default/files/resources/UEFI%20Spec%202.8B%20May%202020.pdf#G16.1016966 are supported at minimum.
        public void OutputString(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, char* str)
        {
            _outputString(handle, str);
        }

        public void QueryMode(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, nuint modeNumber, nuint* columns, nuint* rows)
        {
            _queryMode(handle, modeNumber, columns, rows);
        }

        public void SetMode(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, nuint modeNumber)
        {
            _setMode(handle, modeNumber);
        }

        //Attribute is processed as two nibbles, the lower nibble is for the text/foreground colour and can be any
        //colour in ConsoleColor, however some of the names are different in the uefi spec. The upper nibble is
        //for the background colour and can only be between 0 and 7, i.e. the first 8 colours in ConsoleColor.
        public void SetAttribute(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, nuint attribute)
        {
            _setAttribute(handle, attribute);
        }

        public void ClearScreen(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle)
        {
            _clearScreen(handle);
        }

        // Column and Row must both be greater or equal to zero and less than the maximum window size
        public void SetCursorPosition(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, nuint column, nuint row)
        {
            _setCursorPosition(handle, column, row);
        }

        public void EnableCursor(EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* handle, bool visible)
        {
            _enableCursor(handle, visible);
        }
    }
}
