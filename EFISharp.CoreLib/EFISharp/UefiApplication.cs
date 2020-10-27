using System;
using System.Runtime;
using EFISharp;

public unsafe static class EfiApplication
{
    //TODO Remove function, so far this is required for exe outputtype,
    //dropping that prevents link for finding EfiMain
    public static void Main()
    {

    }

    //TODO Find way to call into other projects that this library does not depend on,
    //this would allow matching c#'s regular main method and allow booting a library
    //without it having a version of this function
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
