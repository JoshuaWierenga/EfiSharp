namespace System
{
	public static partial class Buffer
	{
#if TARGET_AMD64
		//Without comparing the performance of UEFI's MemCopy and dotnet's Memmove, this is just a guess informed by the existing threshold on windows and unix
		private const nuint MemmoveNativeThreshold = 2048;
#endif
	}
}
