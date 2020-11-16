using System;
using EFISharp;
using Internal.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerHelpers
{
    class StartupCodeHelpers
    {
        [System.Runtime.RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2(System.IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2(System.IntPtr frame) { }
        
        [System.Runtime.RuntimeExport("RhpPInvoke")]
        static void RhpPinvoke(System.IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpPInvokeReturn")]
        static void RhpPinvokeReturn(System.IntPtr frame) { }
        
        [System.Runtime.RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L23
        [System.Runtime.RuntimeExport("RhpNewArray")]
        internal static unsafe object RhpNewArray(EEType* pEEType, int length)
        {
            nuint size = pEEType->BaseSize + (nuint)length * pEEType->ComponentSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            IntPtr data = default;
            UefiApplication.SystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, (void**)&data);

            object obj = Unsafe.As<IntPtr, object>(ref data);

            UefiApplication.SystemTable->BootServices->SetMem((void*)data, size, 0);
            SetEEType(data, pEEType);

            byte* b = (byte*)data + sizeof(IntPtr);

            //TODO Check if the casts to IntPtr are required, both byte* and int* are implicitly castable to void*
            UefiApplication.SystemTable->BootServices->CopyMem((void*)(IntPtr)b, (void*)(IntPtr)(&length), sizeof(int));

            return obj;
        }

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L113
        internal static unsafe void SetEEType(IntPtr obj, EEType* type)
        {
            //TODO Check if the cast to IntPtr is required, EEType** is implicitly castable to void*
            UefiApplication.SystemTable->BootServices->CopyMem((void*)obj, (void*)(IntPtr)(&type), (nuint)sizeof(IntPtr));
        }
    }
}
