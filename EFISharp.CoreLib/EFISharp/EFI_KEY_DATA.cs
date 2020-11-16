using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EFI_KEY_DATA
    {
        public readonly EFI_INPUT_KEY Key;
        public readonly EFI_KEY_STATE KeyState;
    }
}