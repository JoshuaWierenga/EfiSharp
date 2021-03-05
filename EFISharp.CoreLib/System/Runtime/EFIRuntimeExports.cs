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
    }
}
