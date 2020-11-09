using System;

namespace EFISharp
{
    [Flags]
    public enum EFI_KEY_TOGGLE_STATE : byte
    {
        EFI_TOGGLE_STATE_VALID = 0x80, 

        EFI_KEY_STATE_EXPOSED = 0x40, 
        EFI_SCROLL_LOCK_ACTIVE = 0x01, 
        EFI_NUM_LOCK_ACTIVE = 0x02, 
        EFI_CAPS_LOCK_ACTIVE = 0x04
    }
}