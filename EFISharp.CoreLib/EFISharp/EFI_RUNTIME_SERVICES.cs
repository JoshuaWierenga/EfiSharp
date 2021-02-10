using System;
using System.Runtime.InteropServices;

namespace EfiSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct EFI_RUNTIME_SERVICES
    {
        private readonly EFI_TABLE_HEADER Hdr;

        // Time Services
        private readonly delegate*<EFI_TIME*, void*, EFI_STATUS> _getTime;

        //TODO Add EFI_TIME_CAPABILITIES and support EFI_RT_PROPERTIES_TABLE configuration table, then update the summary and returns to indicate that.
        /// <summary>
        /// <para>This function returns a time that was valid sometime during the call to the function.</para>
        /// <para>This function should take approximately the same amount of time to read the time each time it is called.</para>
        /// </summary>
        /// <param name="time">Out variable to receive a snapshot of the current time.</param>
        /// <returns>
        /// <para><see cref="EFI_STATUS.EFI_SUCCESS"/> if the operation completed successfully.</para>
        /// <para><see cref="EFI_STATUS.EFI_INVALID_PARAMETER"/> if <paramref name="time"/> is null.</para>
        /// <para><see cref="EFI_STATUS.EFI_DEVICE_ERROR"/> if the time could not be retrieved due to a hardware error.</para>
        /// <para><see cref="EFI_STATUS.EFI_UNSUPPORTED"/> if this call is not supported by this platform at the time the call is made.</para>
        /// </returns>
        public EFI_STATUS GetTime(out EFI_TIME time)
        {
            fixed (EFI_TIME* pTime = &time)
            {
                return _getTime(pTime, null);
            }
        }
    }
}
