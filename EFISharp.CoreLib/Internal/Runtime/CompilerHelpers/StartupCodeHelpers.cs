using System;
using System.Diagnostics;
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

        [System.Runtime.RuntimeExport("RhpThrowEx")]
        static void RhpThrowEx(IntPtr ex)
        {
            RuntimeImports.RhpFallbackFailFast();
        }

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

            UefiApplication.SystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, out IntPtr data);

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

            UefiApplication.SystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, out IntPtr data);

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

        //Port of https://github.com/dotnet/runtimelab/blob/9f1e27d2a2b039d00ec931637bd69f4ab9c03f25/src/coreclr/nativeaot/Runtime/gcrhenv.cpp#L734-L744
        [RuntimeExport("RhCompareObjectContentsAndPadding")]
        private static unsafe bool RhCompareObjectContentsAndPadding(object obj1, object obj2)
        {
            Debug.Assert(obj1.EEType->IsEquivalentTo(obj1.EEType));
            uint cbFields = obj1.GetRawDataSize();

            //Not sure if required but the memcmp implementation below assumes that obj2 >= obj1
            if (cbFields != obj2.GetRawDataSize())
            {
                return false;
            }

            //TODO Does Unsafe.AsPointer work like this?
            byte* pbFields1 = (byte*)*(void**)Unsafe.AsPointer(ref obj1) + sizeof(EEType*);
            byte* pbFields2 = (byte*)*(void**)Unsafe.AsPointer(ref obj2) + sizeof(EEType*);

            for (uint i = 0; i < cbFields; i++)
            {
                if (pbFields1[i] != pbFields2[i])
                {
                    return false;
                }
            }

            return true;
        }

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L113
        internal static unsafe void SetEEType(IntPtr obj, EEType* type)
        {
            UefiApplication.SystemTable->BootServices->CopyMem((void*)obj, &type, (nuint)sizeof(IntPtr));
        }
    }
}
