using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_EVENT
    {
        private readonly void* Event;

        private EFI_EVENT(uint _event)
        {
            Event = &_event;
        }

        private static readonly EFI_EVENT EVT_TIMER = new EFI_EVENT(0x80000000);
        private static readonly EFI_EVENT ENT_RUNTIME = new EFI_EVENT(0x40000000);

        private static readonly EFI_EVENT EVT_NOTIFY_WAIT = new EFI_EVENT(0x00000100);
        private static readonly EFI_EVENT EVT_NOTIFY_SIGNAL = new EFI_EVENT(0x00000200);

        private static readonly EFI_EVENT EVT_SIGNAL_EXIT_BOOT_SERVICES = new EFI_EVENT(0x00000201);
        private static readonly EFI_EVENT EVT_SIGNAL_VIRTUAL_ADDRESS_CHANGE = new EFI_EVENT(0x60000202);
    }
}
