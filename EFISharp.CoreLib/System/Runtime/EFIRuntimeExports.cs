using EfiSharp;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System.Runtime
{
    internal static class EFIRuntimeExports
    {
        //TODO Add RhNewString, this method should work for now however and is exactly what the portable runtime does
        [RuntimeExport("RhNewString")]
        internal static unsafe string RhNewString(EEType* pEEType, int length)
        {
            object newString = InternalCalls.RhpNewArray(pEEType, length);
            return Unsafe.As<object, string>(ref newString);
        }

        [RuntimeExport("memmove")]
        internal static unsafe void* memmove(byte* dmem, byte* smem, nuint size)
        {
            UefiApplication.SystemTable->BootServices->CopyMem(dmem, smem, size);
            return dmem;
        }

        [RuntimeExport("memset")]
        internal static unsafe void* memset(byte* mem, int value, nuint size)
        {
            UefiApplication.SystemTable->BootServices->SetMem(mem, size, (byte)value);
            return mem;
        }

        //TODO Use PalRedHawk cpp files? Should this then be RaiseFailFastException?
        [RuntimeExport("RhpFallbackFailFast")]
        private static unsafe void RhpFallbackFailFast()
        {
            fixed (char* quitLine = "\r\nPress Any Key to Quit.")
            {
                UefiApplication.Out->OutputString(quitLine);
            }

            UefiApplication.SystemTable->BootServices->WaitForEvent(UefiApplication.In->WaitForKeyEx);

            //This will result in this image being unloaded, then open protocols like EFI_SIMPLE_TEXT_EX_PROTOCOL will be closed but memory will remain allocated
            UefiApplication.SystemTable->BootServices->Exit(UefiApplication.ImageHandle, EFI_STATUS.EFI_SUCCESS);
        }
    }
}
