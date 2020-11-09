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
        private readonly IntPtr _pad10;
        private readonly IntPtr _pad11;
        private readonly IntPtr _pad12;
        //This is InstallProtocolInterface and is ignored in favour of OpenProtocol
        private readonly IntPtr _pad13;
        private readonly IntPtr _pad14;
        private readonly IntPtr _pad15;
        //This is HandleProtocol and is ignored in favour of OpenProtocol
        private readonly IntPtr _pad16;
        private readonly void* _pad17;
        private readonly IntPtr _pad18;
        private readonly IntPtr _pad19;
        private readonly IntPtr _pad20;
        private readonly IntPtr _pad21;
        private readonly IntPtr _pad22;
        private readonly IntPtr _pad23;
        private readonly IntPtr _pad24;
        private readonly IntPtr _pad25;
        private readonly IntPtr _pad26;
        private readonly IntPtr _pad27;
        private readonly IntPtr _pad28;
        private readonly IntPtr _pad29;
        private readonly IntPtr _pad30;
        private readonly IntPtr _pad31;
        private readonly IntPtr _openProtocol;

        //TODO Add EFI_EVENT
        public void WaitForEvent(uint NumberOfEvents, IntPtr* Event, uint* Index)
        {
            ((delegate*<uint, IntPtr*, uint*, void>)_waitForEvent)(NumberOfEvents, Event, Index);
        }

        public EFI_STATUS OpenProtocol(EFI_HANDLE handle, EFI_GUID protocol, void** _interface, EFI_HANDLE agentHandle, EFI_HANDLE controllerHandle, EFI_OPEN_PROTOCOL attributes)
        {
            return (EFI_STATUS)((delegate*<EFI_HANDLE, EFI_GUID*, void**, EFI_HANDLE, EFI_HANDLE, EFI_OPEN_PROTOCOL, ulong>)_openProtocol)(handle, &protocol, _interface,
                agentHandle, controllerHandle, attributes);
        }
    }
}
