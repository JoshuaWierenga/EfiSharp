# UEFI runtime for C#

This project is an attempt to allow C# programs to run directly on top of UEFI on either real hardware or in a virtual machine. Currently only a very small amount of the corelib has been ported and so the types of programs that can be used is quite small. This project is supported by the [Native AOT compiler](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT) and is the main reason this is even possible.

## Building

Visual studio is required to build the project, other C# IDEs with support for MSBuild and NuGet might work, though a C++ compiler is required as well. Post an issue if your preferred IDE gives an error while trying to build.\
Within Visual Studio, the ".NET desktop development" workload with the .NET 6 SDK for the main corelib as well as the "Desktop development with C++" workload with Clang Compiler v143/12.0.0 for the mini libc project are required. Note that the current building system has only been tested on Windows, any suggestions for ensuring compatibility with other OSes is appreciated.\
Originally building was done with a separate batch script but its contents have largely been migrated into the MSBuild directory files and so using the build, rebuild and clean options in Visual Studio should be enough. If issues occur while building for UEFI check the troubleshooting section below.

## Running

The `Efi-x64` platform target in Visual Studio should generate a VHD file in `bin\Efi-x64\Release\net6.0\win-x64\native` within the executable project's folder. This file should work with nearly any virtual machine with UEFI support for example HyperV and VirtualBox.\
In the past, HyperV and VirtualBox had problems whenever the VHD file was rebuilt and so the build script can edit VM configuration to avoid this via properties.txt. This appears unnecessary now and so is optional. Additionally, changes to the build system may have broken this, please open an issue if these issues reoccur.\
There is no specific support for other virtual machines through checking for issues with specifically VMware Workstation and QEMU is intended as well as adding native debugging support in the case of QEMU.

## Debugging

To aid in debugging, the corelib has been ported back to Windows so that Visual Studio or another native Windows debugger can be used to step through the code, this can be accessed via the `Windows-x64` platform target. Both `Debug` and `Release` configuration targets should work with `Release` giving an optimised exe that can be run as normal or via running from Visual Studio and `Debug` automatically opening a second instance of Visual Studio and configuring it for debugging. This debugging setup being quite different from regular C# debugging is a known issue and is tracked in issue [#30](https://github.com/JoshuaWierenga/EfiSharp/issues/30).

## Porting existing applications

Care has been taken so that using existing C# applications(that only use supported features) is as simple as possible and in most cases can be done by simply copying the project into the cloned EfiSharp folder or a subfolder of it. Then just open the solution, add the project and build.\
In future, there should be a detailed list of supported APIs and perhaps a list of differences compared to other .NET implementations like framework or core.

## Troubleshooting

There have been/are many weird bugs present with UEFI building and applications in general, here are a selection of them and some tips.
1. Diskpart sometimes fails to generate a VHD.\
This can happen if it is open in another program or if drive X: is taken. In case it fails to generate at all check if another copy exists and if so delete it, if it begins but fails partway through(normally causes X: to be visible in the drive list within explorer) then try the `fixdiskimage` option within `build.bat`. Finally if X: is taken then find and release `X` in `Directory.Build.targets`, this should be properly changeable in future, would also be ideal not to need admin permission to create the VHD.
2. Rebuilt images causing errors in HyperV or VirtualBox.\
As mentioned in the Running section there have previously been issues with these programs getting confused if an image already added to them was then modified. These issues appear to have been fixed but if not then there is partial support for fixing this with the `hyperv` and `virtualbox` options within `build.bat. Keep in mind that these options haven't been used in a while and so maybe broken, regardless the basic information about these issues and the fixes used is present in [#25](https://github.com/JoshuaWierenga/EfiSharp/issues/25) and the [msbuild directory target](https://github.com/JoshuaWierenga/EfiSharp/blob/BuildOverhaul/Directory.Build.targets) file. Open an issue if required.
3. Linker errors after modifying corelib.\
Sometimes adding new functionally to the corelib causes the compiler to require new low level functionally not present and instead of mentioning the new missing functions, the linker will just report every single missing function from the compiler's provided DLLs that are meant for Windows use and hence have many errors on UEFI. A rough fix for this is the `getlinkererrors` option in `build.bat` which removes these dlls and so only reports missing functions that need to be added by the corelib. Do note that in some cases functionally from the compiler's DLLs is actually used, for example, double to integer casting and so there may be functions missing at this step that can be ignored.

## Important changes to make that I am too lazy to open issues for
- [ ] Officially support other virtual machine managers
- [ ] Fix issues with old build script's handling of properties.txt
- [ ] Mention supported APIs
- [ ] Cleanup console PAL for Windows and UEFI
- [ ] Cleanup corelib PAL for WIndows and UEFI(Interop, StartupCodeHelpers, {EFI}RuntimeExports)
- [ ] Fix [interface issues](https://github.com/JoshuaWierenga/EfiSharp/tree/InterfaceSupport), that branch also has more complete TypeCast support
- [ ] Finally merge DateTime support [#17](https://github.com/JoshuaWierenga/EfiSharp/pull/17), update to match [Runtime version](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/src/libraries/System.Private.CoreLib/src/System/DateTime.cs) first
- [ ] Fix static reference fields causing crashes, this was the main reason for the Windows debugging port
