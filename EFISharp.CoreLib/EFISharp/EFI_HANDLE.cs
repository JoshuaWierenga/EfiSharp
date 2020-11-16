using System.Runtime.InteropServices;

namespace EFISharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_HANDLE
    {
        private readonly void* Handle;

        private EFI_HANDLE(void* handle)
        {
            Handle = handle;
        }

        public static readonly EFI_HANDLE NullHandle = new EFI_HANDLE(null);
    }
}
