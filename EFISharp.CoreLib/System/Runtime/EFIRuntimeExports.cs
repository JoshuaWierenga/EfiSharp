using System.Diagnostics;
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

        //From https://github.com/dotnet/runtimelab/blob/5a44950/src/coreclr/nativeaot/Runtime/gcrhenv.cpp#L722-L739
        
        [RuntimeExport("RhpCopyObjectContents")]
        private static unsafe void RhpCopyObjectContents(object objDest, object objSrc)
        {
            nuint cbDest = GetObjectSize(objDest) - (nuint)sizeof(ObjHeader);
            nuint cbSrc = GetObjectSize(objSrc) - (nuint)sizeof(ObjHeader);
            if (cbSrc != cbDest)
            {
                return;
            }

            Debug.Assert(objDest.EETypePtr.HasPointers == objSrc.EETypePtr.HasPointers);

            if (objDest.EETypePtr.HasPointers)
            {
                //TODO Add GCSafeCopyMemoryWithWriteBarrier, could it be based on InlineGCSafeFillMemory since I already did that?
                //GCSafeCopyMemoryWithWriteBarrier(objDest, objSrc, cbDest);
                throw new NotImplementedException("Copying types with GC Pointers is not currently supported.");
            }
            else
            {
                memmove((byte*)Unsafe.AsPointer(ref objDest.GetRawData()), (byte*)Unsafe.AsPointer(ref objSrc.GetRawData()), cbDest);
            }
        }

        //From https://github.com/dotnet/runtimelab/blob/fddede1/src/coreclr/nativeaot/Runtime/ObjectLayout.cpp#L56-L68
        private static nuint GetObjectSize(object obj)
        {
            EETypePtr pEEType = obj.EETypePtr;

            // strings have component size2, all other non-arrays should have 0
            Debug.Assert((pEEType.ComponentSize <= 2) || pEEType.IsArray);

            nuint s = pEEType.BaseSize;
            ushort componentSize = pEEType.ComponentSize;
            if (componentSize > 0)
            {
                s += (nuint)Unsafe.As<Array>(obj).Length * componentSize;
            }
            return s;
        }

        //From https://github.com/dotnet/runtimelab/blob/fddede1/src/coreclr/nativeaot/Runtime/amd64/StubDispatch.asm#L91-L103
        //TODO Implement, somehow this function works fine as is but still, this is not ideal.
        //There is also the question of parameters, I think they are meant to be "IntPtr, IntPtr" but i am not
        //sure since the original function is in asm and the arguments are not documented. 
        //https://github.com/dotnet/runtimelab/blob/fddede1/src/coreclr/nativeaot/Runtime/RuntimeInstance.cpp#L509
        //RuntimeInstance.cpp has RhpInitialDynamicInterfaceDispatch with no parameters but RhpInterfaceDispatchSlow
        //calls RhpCidResolve which needs two IntPtrs. Implicit this?

        //Initial dispatch on an interface when we don't have a cache yet.
        [RuntimeExport("RhpInitialDynamicInterfaceDispatch")]
        private static void RhpInitialDynamicInterfaceDispatch()
        {
            //Just tail call to the cache miss helper.
            //RhpInterfaceDispatchSlow();
        }

        //Cache miss case, call the runtime to resolve the target and update the cache.
        //Use universal transition helper to allow an exception to flow out of resolution
        /*private static void RhpInterfaceDispatchSlow()
        {
            CachedInterfaceDispatch.RhpCidResolve();
            //jmp RhpUniversalTransition_DebugStepTailCall
        }*/
    }
}
