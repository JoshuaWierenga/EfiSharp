using System;
using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
    {
        private readonly IntPtr _pad1;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, char*, EFI_STATUS> _outputString;
        private readonly IntPtr _pad2;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, nuint*, nuint*, EFI_STATUS> _queryMode;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, EFI_STATUS> _setMode;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, EFI_STATUS> _setAttribute;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, EFI_STATUS> _clearScreen;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, nuint, nuint, EFI_STATUS> _setCursorPosition;
        private readonly delegate*<EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL*, bool, EFI_STATUS> _enableCursor;
        public readonly SIMPLE_TEXT_OUTPUT_MODE* Mode;

        /// <param name="buf">Must be a null terminated buffer containing only supported characters, typically chars in https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block) and those
        /// shown in the related definitions section at https://uefi.org/sites/default/files/resources/UEFI%20Spec%202.8B%20May%202020.pdf#G16.1016966 are supported at minimum.</param>
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if <paramref name="buf"/> was output to the device.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device reported an error while attempting to output <paramref name="buf"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if the output device's <see cref="SIMPLE_TEXT_OUTPUT_MODE.Mode"/> was not in a defined text mode.</para>
        /// <para><see cref="EFI_STATUS.EFI_WARN_UNKNOWN_GLYPH"/> if some of the characters in <paramref name="buf"/> could not be rendered and were skipped.</para>
        /// <!-- Non spec returns -->
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="buf"/> was null.</para>
        /// </returns>
        public EFI_STATUS OutputString(char* buf)
        {
            if (buf == null) return EFI_STATUS.EFI_INVALID_PARAMETER;

            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _outputString(_this, buf);
            }
        }

        /// <param name="str">Must be a string containing only supported characters, typically chars in https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block) and those
        /// shown in the related definitions section at https://uefi.org/sites/default/files/resources/UEFI%20Spec%202.8B%20May%202020.pdf#G16.1016966 are supported at minimum.</param>
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if <paramref name="str"/> was output to the device.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device reported an error while attempting to output <paramref name="str"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if the output device's <see cref="SIMPLE_TEXT_OUTPUT_MODE.Mode"/> was not in a defined text mode.</para>
        /// <para><see cref="EFI_STATUS.EFI_WARN_UNKNOWN_GLYPH"/> if some of the characters in <paramref name="str"/> could not be rendered and were skipped.</para>
        /// <!-- Non spec returns -->
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="str"/> was null.</para>
        /// </returns>
        public EFI_STATUS OutputString(string str)
        {
            if (str == null) return EFI_STATUS.EFI_INVALID_PARAMETER;
            fixed (char* pStr = str)
            {
                return OutputString(pStr);
            }
        }

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if information about <see cref="modeNumber"/> was returned.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device had an error and could not complete the request.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if <see cref="modeNumber"/> was not valid.</para>
        /// </returns>
        //TODO use out nuint instead of nuint*
        public EFI_STATUS QueryMode(nuint modeNumber, nuint* columns, nuint* rows)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _queryMode(_this, modeNumber, columns, rows);
            }
        }

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if <see cref="modeNumber"/> was set.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device had an error and could not complete the request.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if <see cref="modeNumber"/> was not valid.</para>
        /// </returns>
        public EFI_STATUS SetMode(nuint modeNumber)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _setMode(_this, modeNumber);
            }
        }

        /// <param name="attribute"><paramref name="attribute"/> is processed as two nibbles, the lower nibble is for the text/foreground colour and can be any
        /// colour in System.Console.ConsoleColor, however some of the names are different in the uefi spec. The upper nibble is
        /// for the background colour and can only be between 0 and 7, i.e. the first 8 colours in System.Console.ConsoleColor.</param>
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if <see cref="Attribute"/> was set.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device had an error and could not complete the request.</para>
        /// </returns>
        public EFI_STATUS SetAttribute(nuint attribute)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _setAttribute(_this, attribute);
            }
        }

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the operation completed successfully.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device had an error and could not complete the request.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if the output device is not in a valid text mode.</para>
        /// </returns>
        public EFI_STATUS ClearScreen()
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _clearScreen(_this);
            }
        }

        /// <param name="column">Must be greater or equal to zero and less than the maximum window size.</param>
        /// <param name="row">Must be greater or equal to zero and less than the maximum window size.</param>
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the operation completed successfully.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device had an error and could not complete the request.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if the output device is not in a valid text mode, or the cursor position is invalid for the <see cref="SIMPLE_TEXT_OUTPUT_MODE.Mode"/>.</para>
        /// </returns>
        public EFI_STATUS SetCursorPosition(nuint column, nuint row)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _setCursorPosition(_this, column, row);
            }
        }

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the operation completed successfully.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the device had an error and could not complete the request or the device does not support changing the cursor mode.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if the output device does not support visibility control of the cursor.</para>
        /// </returns>
        public EFI_STATUS EnableCursor(bool visible)
        {
            fixed (EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* _this = &this)
            {
                return _enableCursor(_this, visible);
            }
        }
    }
}
