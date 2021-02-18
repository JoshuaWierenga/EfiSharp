using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SIMPLE_TEXT_OUTPUT_MODE
    {
        public readonly int MaxMode;
        // current settings
        public readonly int Mode;
        public readonly int Attribute;
        public readonly int CursorColumn;
        public readonly int CursorRow;
        public readonly bool CursorVisible;
    }
}
