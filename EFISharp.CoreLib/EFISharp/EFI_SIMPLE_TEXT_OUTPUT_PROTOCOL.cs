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
        public void OutputString(char* str)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _outputString(_this, str);
            }
        }

        public void QueryMode(nuint modeNumber, nuint* columns, nuint* rows)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _queryMode(_this, modeNumber, columns, rows);
            }
        }

        public void SetMode(nuint modeNumber)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _setMode(_this, modeNumber);
            }
        }

        //Attribute is processed as two nibbles, the lower nibble is for the text/foreground colour and can be any
        //colour in ConsoleColor, however some of the names are different in the uefi spec. The upper nibble is
        //for the background colour and can only be between 0 and 7, i.e. the first 8 colours in ConsoleColor.
        public void SetAttribute(nuint attribute)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _setAttribute(_this, attribute);
            }
        }

        public void ClearScreen()
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _clearScreen(_this);
            }
        }

        // Column and Row must both be greater or equal to zero and less than the maximum window size
        public void SetCursorPosition(nuint column, nuint row)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _setCursorPosition(_this, column, row);
            }
        }

        public void EnableCursor(bool visible)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                _enableCursor(_this, visible);
            }
        }
    }
}
