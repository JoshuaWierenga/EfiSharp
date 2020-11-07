using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SIMPLE_TEXT_OUTPUT_MODE
    {
        private readonly int _pad1;
        public readonly int Mode;
        private readonly int _pad3;
        public readonly int CursorColumn;
        public readonly int CursorRow;
        public readonly bool CursorVisible;

    }
}
