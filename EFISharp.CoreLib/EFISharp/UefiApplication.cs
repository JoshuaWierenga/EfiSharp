using System;
using System.Runtime;
using EFISharp;

public unsafe static class EfiApplication
{
    //TODO Remove function
    [RuntimeExport("main")]
    public static void Main()
    {

    }

    [RuntimeExport("EfiMain")]
    public static long EfiMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        string hello = "Hello world!";
        fixed (char* pHello = hello)
        {
            systemTable->ConOut->OutputString(systemTable->ConOut, pHello);
        }
        while (true) ;
    }
}
