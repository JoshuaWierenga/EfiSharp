using System;
using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_BOOT_SERVICES
    {
        private readonly EFI_TABLE_HEADER Hdr;
        private readonly IntPtr _pad1;
        private readonly IntPtr _pad2;
        private readonly IntPtr _pad3;
        private readonly IntPtr _pad4;
        private readonly IntPtr _pad5;
        private readonly IntPtr _pad6;
        private readonly IntPtr _pad7;
        private readonly IntPtr _pad8;
        private readonly IntPtr _pad9;
        private readonly IntPtr _waitForEvent;

        //TODO Add EFI_EVENT
        public void WaitForEvent(uint NumberOfEvents, IntPtr* Event, uint* Index)
        {
            ((delegate*<uint, IntPtr*, uint*, void>)_waitForEvent)(NumberOfEvents, Event, Index);
        }
    }
}
