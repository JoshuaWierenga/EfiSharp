#pragma warning disable 169

namespace EFISharp
{
    public struct EFI_INPUT_KEY
    {
        private ushort ScanCode;
        public char UnicodeChar;
    }
}
