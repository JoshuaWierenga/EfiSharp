using System.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerServices
{
    public static unsafe partial class Unsafe 
    {
        [Intrinsic]
        public static extern ref T Add<T>(ref T source, int elementOffset);
    }
}
