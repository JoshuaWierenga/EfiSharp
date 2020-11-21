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
        private readonly delegate*<EFI_MEMORY_TYPE, nuint, void**, EFI_STATUS> _allocatePool;
        private readonly delegate*<void*, EFI_STATUS> _freePool;

        // Event & Timer Services
        private readonly IntPtr _pad8;
        private readonly IntPtr _pad9;
        private readonly delegate*<uint, EFI_EVENT*, uint*, EFI_STATUS> _waitForEvent;
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

        //TODO Add summary and params descriptions
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if allocation was successful.</para>
        /// <para><see cref="EFI_STATUS.EFI_OUT_OF_RESOURCES"/> if there was not enough memory free.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="poolType"/> was <see cref="EFI_MEMORY_TYPE.EfiPersistentMemory"/>, <see cref="EFI_MEMORY_TYPE.EfiMaxMemoryType"/> or an undefined type higher than that.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="buffer"/> was null.</para>
        /// </returns>
        public EFI_STATUS AllocatePool(EFI_MEMORY_TYPE poolType, nuint size, void** buffer) =>
            _allocatePool(poolType, size, buffer);

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the freeing was successful.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="buffer"/> was invalid.</para>
        /// </returns>
        public EFI_STATUS FreePool(void* buffer) => _freePool(buffer);

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the event in <paramref name="buffer"/> at <paramref name="index"/> was signaled.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="numberOfEvents"/> is 0.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if the event in <paramref name="buffer"/> at <paramref name="index"/> was of type <see cref="EFI_EVENT.EVT_NOTIFY_SIGNAL"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if the current TPL is not TPL_APPLICATION.</para>
        /// </returns>
        //TODO Add TPL?
        public EFI_STATUS WaitForEvent(uint numberOfEvents, EFI_EVENT* _event, uint* index) =>
            _waitForEvent(numberOfEvents, _event, index);

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if <paramref name="protocol"/> was opened, added to the list of open protocols and returned in <paramref name="_interface"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if <paramref name="handle"/> does not support the given <paramref name="protocol"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="protocol"/> was NULL.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="_interface"/> was NULL and <paramref name="attributes"/> was not <see cref="EFI_OPEN_PROTOCOL.TEST_PROTOCOL"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="handle"/> is NULL.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="attributes"/> is not a legal value.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="agentHandle"/> is null and <paramref name="attributes"/> is one of: <see cref="EFI_OPEN_PROTOCOL.BY_CHILD_CONTROLLER"/>, <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>, <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/>, or <see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/></para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="controllerHandle"/> is null and <paramref name="attributes"/> is one of: <see cref="EFI_OPEN_PROTOCOL.BY_CHILD_CONTROLLER"/>, <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>, or <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/></para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="handle"/> is equal to <paramref name="controllerHandle"/> and <paramref name="attributes"/> is <see cref="EFI_OPEN_PROTOCOL.BY_CHILD_CONTROLLER"/></para>
        /// <para><see cref="EFI_STATUS.EFI_ACCESS_DENIED"/> if <paramref name="attributes"/> is <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>, or <see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/> and the current list of open protocols contains one with an attribute of either <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/>, or <see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/></para>
        /// <para><see cref="EFI_STATUS.EFI_ACCESS_DENIED"/> if <paramref name="attributes"/> is <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/> and the current list of open protocols contains one with an attribute of <see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_ACCESS_DENIED"/> if <paramref name="attributes"/> is <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>, or <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/> and the current list of open protocols contains one with the same attribute and an agent handle that is different to <paramref name="agentHandle"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_ACCESS_DENIED"/> if <paramref name="attributes"/> is <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/> or <see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/> and the current list of open protocols contains one with an attribute of <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/> that could not be removed when EFI_BOOT_SERVICES.DisconnectController() was called on it.</para>
        /// <para><see cref="EFI_STATUS.EFI_ALREADY_STARTED"/> if <paramref name="attributes"/> is <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>, or <see cref="EFI_OPEN_PROTOCOL.BY_DRIVER"/>|<see cref="EFI_OPEN_PROTOCOL.EXCLUSIVE"/> and the current list of open protocols contains one with the same attribute and an agent handle that is the same as <paramref name="agentHandle"/>.</para>
        /// </returns>
        public EFI_STATUS OpenProtocol(EFI_HANDLE handle, EFI_GUID protocol, void** _interface, EFI_HANDLE agentHandle,
            EFI_HANDLE controllerHandle, EFI_OPEN_PROTOCOL attributes) => _openProtocol(handle, &protocol, _interface,
            agentHandle, controllerHandle, attributes);

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
