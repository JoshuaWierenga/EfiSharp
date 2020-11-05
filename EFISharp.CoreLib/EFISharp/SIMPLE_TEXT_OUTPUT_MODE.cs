using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SIMPLE_TEXT_OUTPUT_MODE
    {
        private readonly IntPtr _pad1;
        private readonly IntPtr _pad2;
        private readonly IntPtr _pad3;
        private readonly IntPtr _pad4;
        private readonly IntPtr _pad5;
        public readonly bool CursorVisible;

    }
}