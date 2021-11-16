using Internal.Runtime.CompilerServices;

namespace System
{
    public unsafe partial class Object
    {
        //From https://github.com/Michael-Kelley/RoseOS/blob/ecd805014a/CoreLib/System/Object.cs#L21
        public void Free()
        {
            object obj = this;
            IntPtr pObj = Unsafe.As<object, IntPtr>(ref obj);
#if RELEASE
            Interop.Kernel32.LocalFree(pObj);
#elif EFI_RELEASE
            EfiSharp.UefiApplication.SystemTable->BootServices->FreePool(pObj);
#endif
        }
    }
}
