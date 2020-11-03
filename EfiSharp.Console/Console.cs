using System.Runtime.CompilerServices;

namespace System
{
    public unsafe class Console
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        //TODO Add Nullable?
        public static void Write(string value)
        {
            fixed (char* pValue = value)
            {
                UefiApplication.SystemTable->ConOut->OutputString(UefiApplication.SystemTable->ConOut, pValue);
            }
        }
    }
}