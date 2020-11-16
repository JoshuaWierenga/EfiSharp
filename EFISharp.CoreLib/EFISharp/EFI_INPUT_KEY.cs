#pragma warning disable 169

namespace EfiSharp
{
    public readonly struct EFI_INPUT_KEY
    {
        private readonly ushort ScanCode;
        public readonly char UnicodeChar;
    }
}
