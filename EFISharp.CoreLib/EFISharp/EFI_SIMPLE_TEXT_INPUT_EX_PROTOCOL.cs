using System;
using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL
    {
        private readonly IntPtr _pad;
        private readonly delegate*<EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL*, EFI_KEY_DATA*, EFI_STATUS> _readKeyStrokeEx;
        public readonly EFI_EVENT _waitForKeyEx;
        //readonly IntPtr _setState;

        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the keystroke information was returned.</para>
        /// <para><see cref="EFI_STATUS.EFI_NOT_READY"/> if there was no keystroke data available. <paramref name="key"/>.KeyState values are still exposed if <paramref name="key"/>.KeyState.KeyToggleState has <see cref="EFI_KEY_TOGGLE_STATE.EFI_KEY_STATE_EXPOSED"/> set.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the keystroke information was not returned due to hardware errors.</para>
        /// </returns>
        public EFI_STATUS ReadKeyStrokeEx(out EFI_KEY_DATA key)
        {
            fixed (EFI_SIMPLE_TEXT_INPUT_EX_PROTOCOL* _this = &this)
            {
                fixed (EFI_KEY_DATA* pKey = &key)
                {
                    return _readKeyStrokeEx(_this, pKey);
                }
            }
        }

        public static readonly EFI_GUID Guid = new(0xdd9e7534, 0x7762, 0x4698, 0x8c, 0x14, 0xf5, 0x85, 0x17, 0xa6, 0x25, 0xaa);
    }
}
