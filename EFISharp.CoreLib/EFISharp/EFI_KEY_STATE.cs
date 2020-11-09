using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EFI_KEY_STATE
    {
        public readonly KeyShiftState KeyShiftState;
        public readonly EFI_KEY_TOGGLE_STATE KeyToggleState;
    }
}