using System;
using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_BOOT_SERVICES
    {
        private readonly EFI_TABLE_HEADER Hdr;
        private readonly IntPtr _pad1;
        private readonly IntPtr _pad2;

        // Memory Services
        private readonly IntPtr _pad3;
        private readonly IntPtr _pad4;
        private readonly IntPtr _pad5;
        private readonly delegate*<EFI_MEMORY_TYPE, nuint, void**, void> _allocatePool;
        private readonly delegate*<void*, void> _freePool;

        // Event & Timer Services
        private readonly IntPtr _pad8;
        private readonly IntPtr _pad9;
        private readonly delegate*<uint, IntPtr*, uint*, void> _waitForEvent;
        private readonly IntPtr _pad10;
        private readonly IntPtr _pad11;
        private readonly IntPtr _pad12;

        // Protocol Handler Services
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

        // Open and Close Protocol Services
        private readonly delegate*<EFI_HANDLE, EFI_GUID*, void**, EFI_HANDLE, EFI_HANDLE, EFI_OPEN_PROTOCOL, EFI_STATUS> _openProtocol;
        private readonly IntPtr _pad32;
        private readonly IntPtr _pad33;

        private readonly IntPtr _pad34;
        private readonly IntPtr _pad35;
        private readonly IntPtr _pad36;
        private readonly IntPtr _pad37;
        private readonly IntPtr _pad38;
        private readonly IntPtr _pad39;

        //Miscellaneous Services
        private readonly delegate*<void*, void*, nuint, void> _copyMem;
        private readonly delegate*<void*, nuint, byte, void> _setMem;

        public void AllocatePool(EFI_MEMORY_TYPE poolType, nuint size, void** buffer)
        {
            _allocatePool(poolType, size, buffer);
        }

        public void FreePool(void* buffer)
        { 
            _freePool(buffer);
        }

        //TODO Add EFI_EVENT
        public void WaitForEvent(uint NumberOfEvents, IntPtr* Event, uint* Index)
        {
            _waitForEvent(NumberOfEvents, Event, Index);
        }

        public EFI_STATUS OpenProtocol(EFI_HANDLE handle, EFI_GUID protocol, void** _interface, EFI_HANDLE agentHandle, EFI_HANDLE controllerHandle, EFI_OPEN_PROTOCOL attributes)
        {
            return _openProtocol(handle, &protocol, _interface,
                agentHandle, controllerHandle, attributes);
        }

        public void CopyMem(void* destination, void* source, nuint length)
        {
            _copyMem(destination, source, length);
        }

        public void SetMem(void* buffer, nuint size, byte value)
        {
            _setMem(buffer, size, value);
        }
    }
}
