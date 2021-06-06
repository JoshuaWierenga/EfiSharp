using Internal.Runtime.CompilerServices;

namespace System
{
    public unsafe partial class Object
    {
        //From https://github.com/Michael-Kelley/RoseOS/blob/ecd805014a/CoreLib/System/Object.cs#L21
        public void Free()
        {
            object obj = this;
            EfiSharp.UefiApplication.SystemTable->BootServices->FreePool(Unsafe.As<object, IntPtr>(ref obj));
        }
    }
}
