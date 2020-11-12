namespace Internal.Runtime.CompilerHelpers
{
    //TODO https://github.com/Michael-Kelley/RoseOS/blob/master/CoreLib/Internal/Runtime/CompilerHelpers/StartupCodeHelpers.cs, check if using that requires asking about licence, also https://github.com/dotnet/corert/issues/8075#issuecomment-610968591
    class StartupCodeHelpers
    {
        [System.Runtime.RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2(System.IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2(System.IntPtr frame) { }
        
        [System.Runtime.RuntimeExport("RhpPInvoke")]
        static void RhpPinvoke(System.IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpPInvokeReturn")]
        static void RhpPinvokeReturn(System.IntPtr frame) { }
        
        [System.Runtime.RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }

        [System.Runtime.RuntimeExport("RhpNewArray")]
        static void RhpNewArray() { }
    }
}
