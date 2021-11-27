using System;
using System.Runtime;
#if EFI
using EfiSharp;
#endif
using Internal.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerHelpers
{
    //TODO Remove and replace with c++?
    //At a minimum move this file elsewhere since there is a corelib file with this name
    //Also clean it up, the changes required for windows support have made it quite messy. Merge with EFIRuntimeExports?
    partial class StartupCodeHelpers
    {
        [RuntimeExport("RhpReversePInvoke")]
        static void RhpReversePInvoke(IntPtr frame) { }
        [RuntimeExport("RhpReversePInvokeReturn")]
        static void RhpReversePInvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpPInvoke")]
        static void RhpPinvoke(IntPtr frame) { }
        [RuntimeExport("RhpPInvokeReturn")]
        static void RhpPinvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpThrowEx")]
        static unsafe void RhpThrowEx(Exception ex)
        {
            Console.Write("\r\n\nEXCEPTION: ");

            if (ex.Message == null)
            {
                Console.Write("Unknown");

            }
            else
            {

                Console.Write(ex.Message);
            }

            if (ex is ArgumentException {ParamName: { } name})
            {
                Console.Write(": " + name);
            }

            if (ex.Source != null)
            {
                Console.Write(": " + ex.Source);
            }

            if (ex.HelpLink != null)
            {
                Console.Write(": " + ex.HelpLink);
            }

            RuntimeImports.RhpFallbackFailFast();
        }

        /*[RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }*/

        [RuntimeExport("RhpNewFast")]
        private static unsafe object RhpNewFast(MethodTable* pEEType)
        {
            nuint size = pEEType->BaseSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            IntPtr data;
#if WINDOWS
            data = Interop.Kernel32.LocalAlloc(0, size);
#elif EFI
            UefiApplication.SystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, out data);
#endif
            object obj = Unsafe.As<IntPtr, object>(ref data);
#if WINDOWS
            long* p = (long*)data;
            nuint count = size / 8;
            nuint rem = size % 8;

            for (ulong i = 0UL; i < count; i++)
                p[i] = 0;

            if (rem > 0)
            {
                byte* b = (byte*)data;
                b += count * sizeof(long);

                for (ulong i = 0UL; i < rem; i++)
                    b[i] = 0;
            }
#elif EFI
            UefiApplication.SystemTable->BootServices->SetMem((void*)data, size, 0);
#endif
            SetEEType(data, pEEType);

            return obj;
        }

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L38
        [RuntimeExport("RhpNewArray")]
        private static unsafe object RhpNewArray(MethodTable* pEEType, int length)
        {
            nuint size = pEEType->BaseSize + (nuint)length * pEEType->ComponentSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            IntPtr data;
#if WINDOWS
            data = Interop.Kernel32.LocalAlloc(0, size);
#elif EFI
            UefiApplication.SystemTable->BootServices->AllocatePool(EFI_MEMORY_TYPE.EfiLoaderData, size, out data);
#endif

            object obj = Unsafe.As<IntPtr, object>(ref data);
#if WINDOWS
            long* p = (long*)data;
            nuint count = size / 8;
            nuint rem = size % 8;

            for (ulong i = 0UL; i < count; i++)
                p[i] = 0;

            if (rem > 0)
            {
                byte* b = (byte*)data;
                b += count * sizeof(long);

                for (ulong i = 0UL; i < rem; i++)
                    b[i] = 0;
            }
#elif EFI
            UefiApplication.SystemTable->BootServices->SetMem((void*)data, size, 0);
#endif
            SetEEType(data, pEEType);

            byte* b2 = (byte*)data + sizeof(IntPtr);

#if WINDOWS
            long* d = (long*)b2;
            long* s = (long*)&length;
            ulong count2 = (ulong)sizeof(int) / 8;
            ulong rem2 = (ulong)sizeof(int) % 8;

            for (ulong i = 0UL; i < count2; i++)
                d[i] = s[i];

            if (rem2 > 0)
            {
                byte* bd = b2;
                byte* bs = (byte*)&length;

                bd += count2 * sizeof(long);
                bs += count2 * sizeof(long);

                for (var i = 0UL; i < rem2; i++)
                    bd[i] = bs[i];
            }
#elif EFI
            UefiApplication.SystemTable->BootServices->CopyMem(b2, &length, sizeof(int));
#endif

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
            fixed (int* n = &array._numComponents)
            {
                var ptr = (byte*)n;
                ptr += 8;   // Array length is padded to 8 bytes on 64-bit
                ptr += index * array.m_pEEType->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
                var pp = (IntPtr*)ptr;
                *pp = Unsafe.As<object, IntPtr>(ref obj);
            }
        }

        [RuntimeExport("RhTypeCast_IsInstanceOfClass")]
        static unsafe object RhTypeCast_IsInstanceOfClass(MethodTable* pTargetType, object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (pTargetType == obj.m_pEEType)
            {
                return obj;
            }

            MethodTable* bt = obj.m_pEEType->RawBaseType;

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
        /*[RuntimeExport("RhCompareObjectContentsAndPadding")]
        private static unsafe bool RhCompareObjectContentsAndPadding(object obj1, object obj2)
        {
            Debug.Assert(obj1.MethodTable->IsEquivalentTo(obj1.MethodTable));
            uint cbFields = obj1.GetRawDataSize();

            //Not sure if required but the memcmp implementation below assumes that obj2 >= obj1
            if (cbFields != obj2.GetRawDataSize())
            {
                return false;
            }

            //TODO Does Unsafe.AsPointer work like this?
            byte* pbFields1 = (byte*)*(void**)Unsafe.AsPointer(ref obj1) + sizeof(MethodTable*);
            byte* pbFields2 = (byte*)*(void**)Unsafe.AsPointer(ref obj2) + sizeof(MethodTable*);

            for (uint i = 0; i < cbFields; i++)
            {
                if (pbFields1[i] != pbFields2[i])
                {
                    return false;
                }
            }

            return true;
        }*/

        //From https://github.com/Michael-Kelley/RoseOS/blob/8105be1c1e/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs#L113
        internal static unsafe void SetEEType(IntPtr obj, MethodTable* type)
        {
#if WINDOWS
            long* d = (long*)obj;
            long* s = (long*)&type;
            ulong count = (ulong)sizeof(IntPtr) / 8;
            ulong rem = (ulong)sizeof(IntPtr) % 8;

            for (ulong i = 0UL; i < count; i++)
                d[i] = s[i];

            if (rem > 0)
            {
                byte* bd = (byte*)obj;
                byte* bs = (byte*)&type;

                bd += count * sizeof(long);
                bs += count * sizeof(long);

                for (var i = 0UL; i < rem; i++)
                    bd[i] = bs[i];
            }
#elif EFI
            UefiApplication.SystemTable->BootServices->CopyMem((void*)obj, &type, (nuint)sizeof(IntPtr));
#endif
        }
    }
}
