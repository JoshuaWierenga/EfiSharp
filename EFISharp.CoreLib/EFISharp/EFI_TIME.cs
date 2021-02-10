using System.Runtime.InteropServices;

namespace EfiSharp
{

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EFI_TIME
    {
        public readonly ushort Year;
        public readonly byte Month;
        public readonly byte Day;
        public readonly byte Hour;
        public readonly byte Minute;
        public readonly byte Second;
        public readonly byte Pad1;
        public readonly uint Nanosecond;
    }
}
