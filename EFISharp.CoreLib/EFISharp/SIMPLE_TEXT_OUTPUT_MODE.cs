using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SIMPLE_TEXT_OUTPUT_MODE
    {
        private readonly int _pad1;
        private readonly int _pad2;
        private readonly int _pad3;
        public readonly int CursorColumn;
        public readonly int CursorRow;
        public readonly bool CursorVisible;

    }
}