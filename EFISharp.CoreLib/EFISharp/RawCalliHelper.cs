using System;
using System.Runtime.InteropServices;

namespace EFISharp
{

    [McgIntrinsicsAttribute]
    internal class RawCalliHelper
    {
        public static unsafe void StdCall<T, U>(IntPtr pfn, T* arg1, U* arg2) where T : unmanaged where U : unmanaged
        {
            // This will be filled in by an IL transform
        }
    }
}
