using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class Interop
{
    public class Sys
    {
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [DllImport("*", EntryPoint = "RhpFallbackFailFast")]
        internal static extern void Abort();
    }
}
