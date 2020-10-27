using System.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerServices
{
    //TODO add MethodImplAttribute, MethodImplOptions and Exception support and use nativeaot version of this file
    public static unsafe partial class Unsafe 
    {
        [Intrinsic]
        public static extern ref T Add<T>(ref T source, int elementOffset);
    }
}
