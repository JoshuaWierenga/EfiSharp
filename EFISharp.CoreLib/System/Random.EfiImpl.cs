using EfiSharp;

namespace System
{
    public partial class Random
    {
        //TODO Use two point form to map to range, this should work for integer and floating point results 
        private sealed unsafe class EfiImpl : ImplBase
        {
            private static EFI_RNG_PROTOCOL* rand = null;
            public EfiImpl()
            {
                if (UefiApplication.SystemTable->BootServices->LocateHandle(EFI_LOCATE_SEARCH_TYPE.ByProtocol,
                    EFI_RNG_PROTOCOL.Guid, out EFI_HANDLE[] buffer) != EFI_STATUS.EFI_SUCCESS)
                {
                    buffer?.Dispose();
                    return;
                }

                if (buffer.Length == 0 || UefiApplication.SystemTable->BootServices->OpenProtocol(buffer[0],
                    EFI_RNG_PROTOCOL.Guid, out void* _interface, UefiApplication.ImageHandle, EFI_HANDLE.NullHandle,
                    EFI_OPEN_PROTOCOL.GET_PROTOCOL) != EFI_STATUS.EFI_SUCCESS)
                {
                    rand = null;
                }
                else
                {
                    rand = (EFI_RNG_PROTOCOL*)_interface;
                }

                buffer.Dispose();
            }

            public override void NextBytes(byte[] buffer)
            {
                if (rand != null)
                {
                    rand->GetRNG(buffer);
                }
            }
        }
    }
}
