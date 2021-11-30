using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal partial class Interop
{
    internal class Sys
    {
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [DllImport("*", EntryPoint = "RhpFallbackFailFast")]
        internal static extern void Abort();
    }
}
