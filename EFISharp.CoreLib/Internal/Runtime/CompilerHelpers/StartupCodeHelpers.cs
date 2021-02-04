using System;
using System.Runtime;
using EfiSharp;
using Internal.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerHelpers
{
    class StartupCodeHelpers
    {
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
