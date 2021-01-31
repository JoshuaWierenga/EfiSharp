using System;
using System.Runtime;
using EfiSharp;
using Internal.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerHelpers
{
    class StartupCodeHelpers
    {
        [RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2(IntPtr frame) { }
        [RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2(IntPtr frame) { }

        [RuntimeExport("RhpPInvoke")]
        static void RhpPinvoke(IntPtr frame) { }
        [RuntimeExport("RhpPInvokeReturn")]
        static void RhpPinvokeReturn(IntPtr frame) { }

        [RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }

        [RuntimeExport("memset")]
        static unsafe void MemSet(byte* ptr, int c, int count)
        {
            for (byte* p = ptr; p < ptr + count; p++)
            {
                *p = (byte)c;
            }
        }

        [RuntimeExport("RhpNewFast")]
        private static unsafe object RhpNewFast(EEType* pEEType)
        {
            nuint size = pEEType->BaseSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            IntPtr data = default;
            UefiApplication.SystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, (void**)&data);

            object obj = Unsafe.As<IntPtr, object>(ref data);
            UefiApplication.SystemTable->BootServices->SetMem((void*)data, size, 0);
            SetEEType(data, pEEType);

            return obj;
        }

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L38
        [RuntimeExport("RhpNewArray")]
        private static unsafe object RhpNewArray(EEType* pEEType, int length)
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

            UefiApplication.SystemTable->BootServices->CopyMem(b, &length, sizeof(int));

            return obj;
        }

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L66
        [RuntimeExport("RhpAssignRef")]
        private static unsafe void RhpAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpByRefAssignRef")]
        static unsafe void RhpByRefAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpCheckedAssignRef")]
        static unsafe void RhpCheckedAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        //TODO Replace with TypeCast.StelemRef
        [RuntimeExport("RhpStelemRef")]
        static unsafe void RhpStelemRef(Array array, int index, object obj)
        {
            //TODO Add generic GetPinnableReference so that array.Length can be used inside of fixed
            fixed(int * n = &array._numComponents) {
                var ptr = (byte*)n;
                ptr += 8;   // Array length is padded to 8 bytes on 64-bit
                ptr += index * array.m_pEEType->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
                var pp = (IntPtr*)ptr;
                *pp = Unsafe.As<object, IntPtr>(ref obj);
            }
        }

        [RuntimeExport("RhTypeCast_IsInstanceOfClass")]
        static unsafe object RhTypeCast_IsInstanceOfClass(EEType* pTargetType, object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (pTargetType == obj.m_pEEType)
            {
                return obj;
            }

            EEType* bt = obj.m_pEEType->RawBaseType;

            while (true)
            {
                if (bt == null)
                {
                    return null;
                }

                if (pTargetType == bt)
                {
                    return obj;
                }

                bt = bt->RawBaseType;
            }
        }


        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L113
        internal static unsafe void SetEEType(IntPtr obj, EEType* type)
        {
            UefiApplication.SystemTable->BootServices->CopyMem((void*)obj, &type, (nuint)sizeof(IntPtr));
        }
    }
}
