using System;
using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_RNG_PROTOCOL
    {
        private readonly IntPtr _pad;
        //TODO Replace void* with EFI_RNG_ALGORITHM*
        private readonly delegate*<EFI_RNG_PROTOCOL*, void*, nuint, byte*, EFI_STATUS> _getRNG;

        //TODO Support EFI_RNG_ALGORITHM
        //TODO Decide if rngValue should be created in this function or by the caller
        /// <summary>
        /// <para>This function fills the <paramref name="rngValue"/> buffer with random bytes<!-- from the specified RNG algorithm-->.</para>
        /// <para>The driver must not reuse random bytes across calls to this function.</para>
        /// <para>It is the caller’s responsibility to allocate the <paramref name="rngValue"/> buffer.</para>
        /// </summary>
        /// <param name="rngValue">A caller-allocated memory buffer filled by the driver with the resulting RNG value.</param>
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the RNG value was returned successfully.</para>
        /// <!--<para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if algorithm specified by RNGAlgorithm is not supported by this driver.</para>-->
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if an RNG value could not be retrieved due to a hardware or firmware error.</para>
        /// <para><see cref="EFI_STATUS.EFI_NOT_READY"/> if there is not enough random data available to satisfy the length of <paramref name="rngValue"/>.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="rngValue"/>'s length is zero.</para>
        /// </returns>
        public EFI_STATUS GetRNG(byte[] rngValue)
        {
            fixed (EFI_RNG_PROTOCOL* _this = &this)
            {
                //TODO FIX size given needs to be in bytes, rngValue.Length * sizeof(byte), might be fine in this case since length is the number of bytes
                fixed (byte* prngValue = rngValue)
                {
                    return _getRNG(_this, null, (nuint)rngValue.Length, prngValue);
                }
            }
        }

        public static readonly EFI_GUID Guid = new(0x3152bca5, 0xeade, 0x433d, 0x86, 0x2e, 0xc0, 0x1c, 0xdc, 0x29, 0x1f, 0x44);
    }
}
