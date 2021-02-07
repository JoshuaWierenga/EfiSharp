using System.Runtime.InteropServices;

namespace EfiSharp
{
    //TODO fix arrays and use for Data4
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EFI_GUID
    {
        public readonly uint Data1;
        public readonly ushort Data2;
        public readonly ushort Data3;
        public readonly byte Data41;
        public readonly byte Data42;
        public readonly byte Data43;
        public readonly byte Data44;
        public readonly byte Data45;
        public readonly byte Data46;
        public readonly byte Data47;
        public readonly byte Data48;

        public EFI_GUID(uint data1, ushort data2, ushort data3, byte data41, byte data42, byte data43, byte data44, byte data45, byte data46, byte data47, byte data48)
        {
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
            Data41 = data41;
            Data42 = data42;
            Data43 = data43;
            Data44 = data44;
            Data45 = data45;
            Data46 = data46;
            Data47 = data47;
            Data48 = data48;
        }
    }
}
