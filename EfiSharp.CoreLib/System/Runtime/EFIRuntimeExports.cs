#if EFI
using EfiSharp;
#endif
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System.Runtime
{
    //TODO Clean this file up, the changes required for windows support have made it quite messy
    internal static class EFIRuntimeExports
    {
#if DEBUG && WINDOWS
        [RuntimeExport("RhpMemoryBarrier")]
        static void RhpMemoryBarrier()
        {

        }

        [RuntimeExport("RhpLockCmpXchg32")]
        static void RhpLockCmpXchg32()
        {

        }

        [RuntimeExport("RhBox")]
        static void RhBox()
        {

        }
#endif

        //TODO Add RhNewString, this method should work for now however and is exactly what the portable runtime does
        [RuntimeExport("RhNewString")]
        internal static unsafe string RhNewString(MethodTable* pEEType, int length)
        {
            object newString = InternalCalls.RhpNewArray(pEEType, length);
            return Unsafe.As<object, string>(ref newString);
        }

        [RuntimeExport("memmove")]
        internal static unsafe void* memmove(byte* dmem, byte* smem, nuint size)
        {
#if WINDOWS
            long* d = (long*)dmem;
            long* s = (long*)smem;
            nuint count = size/ 8;
            nuint rem = size % 8;

            for (ulong i = 0UL; i < count; i++)
                d[i] = s[i];

            if (rem > 0)
            {
                byte* bd = dmem;
                byte* bs = (byte*)&smem;

                bd += count * sizeof(long);
                bs += count * sizeof(long);

                for (var i = 0UL; i < rem; i++)
                    bd[i] = bs[i];
            }
#elif EFI
            UefiApplication.SystemTable->BootServices->CopyMem(dmem, smem, size);
#endif
            return dmem;
        }

        [RuntimeExport("memset")]
        internal static unsafe void* memset(byte* mem, int value, nuint size)
        {
#if WINDOWS
            for (byte* p = mem; p < mem + size; p++)
                *p = (byte)value;
#elif EFI
            UefiApplication.SystemTable->BootServices->SetMem(mem, size, (byte)value);
#endif
            return mem;
        }

        //TODO Use PalRedHawk cpp files? Should this then be RaiseFailFastException?
        [RuntimeExport("RhpFallbackFailFast")]
        private static unsafe void RhpFallbackFailFast()
        {
#if WINDOWS
            while (true) ;
#elif EFI
            fixed (char* quitLine = "\r\nPress Any Key to Quit.")
            {
                UefiApplication.Out->OutputString(quitLine);
            }

            UefiApplication.SystemTable->BootServices->WaitForEvent(UefiApplication.In->WaitForKeyEx);

            //This will result in this image being unloaded, then open protocols like EFI_SIMPLE_TEXT_EX_PROTOCOL will be closed but memory will remain allocated
            UefiApplication.SystemTable->BootServices->Exit(UefiApplication.ImageHandle, EFI_STATUS.EFI_SUCCESS);
#endif
        }
    }
}
