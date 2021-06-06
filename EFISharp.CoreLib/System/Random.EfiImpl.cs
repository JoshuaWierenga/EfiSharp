using EfiSharp;

namespace System
{
    public partial class Random
    {
        private sealed unsafe class EfiImpl : ImplBase
        {
            private static EFI_RNG_PROTOCOL* _rand = null;
            
            public EfiImpl()
            {
                if (UefiApplication.SystemTable->BootServices->LocateHandle(EFI_LOCATE_SEARCH_TYPE.ByProtocol,
                    EFI_RNG_PROTOCOL.Guid, out EFI_HANDLE[] buffer) != EFI_STATUS.EFI_SUCCESS)
                {
                    buffer?.Free();
                    return;
                }

                if (buffer.Length != 0 && UefiApplication.SystemTable->BootServices->OpenProtocol(buffer[0],
                    EFI_RNG_PROTOCOL.Guid, out void* _interface, UefiApplication.ImageHandle, EFI_HANDLE.NullHandle,
                    EFI_OPEN_PROTOCOL.GET_PROTOCOL) == EFI_STATUS.EFI_SUCCESS)
                {
                    _rand = (EFI_RNG_PROTOCOL*) _interface;
                }

                buffer.Free();
            }

            public override int Next()
            {
                byte[] randomNumArray = new byte[4];
                NextBytes(randomNumArray);

                int randomNum = BitConverter.ToInt32(randomNumArray, 0);

                if (randomNum < 0)
                {
                    randomNum = -randomNum;
                }

                randomNum.Free();
                return randomNum;
            }

            public override int Next(int maxValue) => (int) (Sample() * maxValue);

            public override int Next(int minValue, int maxValue) =>
                (int)(Sample() * ((long)maxValue - minValue)) + minValue;

            public override long NextInt64()
            {
                byte[] randomNumArray = new byte[8];
                NextBytes(randomNumArray);

                long randomNum = BitConverter.ToInt64(randomNumArray, 0);

                if (randomNum < 0)
                {
                    randomNum = -randomNum;
                }

                randomNumArray.Free();
                return randomNum;
            }

            public override long NextInt64(long maxValue) => (long)(Sample() * maxValue);

            //This works well enough but breaks down as maxValue - minValue approaches ulong.MaxValue, the result is still in range but specific values like 0 become very common
            public override long NextInt64(long minValue, long maxValue)
            {
                ulong range;
                if (minValue < 0 && maxValue > 0)
                {
                    //Option 1:
                    //max is big, min is -small
                    //max - min = big - -small = big + small
                    //Option 2:
                    //max is small, min is -big
                    //max - min = small - -big = small + big
                    range = (ulong)maxValue + (ulong)(-minValue);
                }
                else
                {
                    //Option 3
                    //max is -small, min is -big
                    //max - min = -small - -big = big - small
                    //Option 4
                    //max is big, min is small
                    //max - min = big - small
                    range = (ulong)(maxValue - minValue);
                }

                return (long)(Sample() * range) + minValue;
            }

            public override float NextSingle() => (float) Sample();

            public override double NextDouble() => Sample();

            public override void NextBytes(byte[] buffer)
            {
                if (_rand != null)
                {
                    _rand->GetRNG(buffer);
                }
            }

            public override double Sample() => Next() * (1.0 / int.MaxValue);
        }
    }
}
