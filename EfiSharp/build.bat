@echo off

set help=F
if "%1"=="help" set help=T
if "%1"=="h" set help=T

if "%help%"=="T" (
	echo This program allows building EfiSharp and also preparing the built image for either hyperv or virtualbox.
	echo.
	echo Available arguments:
	echo help: Shows this text. 
	echo VM Management:
	echo ALL OPTIONS ARE CURRENTLY BROKEN ON V6
	echo hyperv: Set permissions for the image using icacls so that an existing vm can open a rebuilt image file without manually readding it.
	echo virtualbox: Reconfigures an existing vm using VBoxManage to allow opening a rebuilt image file without manually readding it.
	echo Note that both of these options assume that the image which is stored in EfiSharp\bin\x64\Release\net5.0\win-x64\native\EfiSharp.vhd has 
	echo already been added manually to a vm and that the instructions mentioned at the bottom of EfiSharp.csproj have be followed.
	echo Debug:
	echo getlinkererrors: Skips setting linker arguments so that a reasonable error list is shown. The normal build process shows 50+ errors on 
	echo build failure and often does not show the actual error^(s^).
	echo.
	echo By Joshua Wierenga on 5/02/2021
	
	goto :end
)

rem EfiSharp.CoreLib, EfiSharp.Console and EfiSharp compilation to make EfiSharp.dll
dotnet build -r win-x64 -c Release --no-incremental
if errorlevel 1 (
	exit /b %errorlevel%
)
rem EFiSharp.Native compliation to make EFiSharp.Native.lib
msbuild ..\EfiSharp.Native\EFiSharp.Native.vcxproj /p:configuration=release

echo.
echo.
echo !NOTICE!: The next command will error quite a bit but will still work
timeout 5
rem EfiSharp.dll compilation to make Efisharp.obj + errors on an attempt to link
dotnet publish -r win-x64 -c Release --no-build
rem EfiSharp.obj and EfiSharp.Native.lib linking to make EfiSharp.efi
link "obj\x64\Release\net5.0\win-x64\native\EfiSharp.obj" /OUT:"bin\x64\Release\net5.0\win-x64\native\EfiSharp.efi" "..\EfiSharp.Native\x64\release\EFiSharp.Native.lib" /subsystem:EFI_APPLICATION /entry:EfiMain

rem Making EfiSharp.vhd
msbuild /t:GenerateVirtualDisk /p:RuntimeIdentifier=win-x64 /p:configuration=release

rem if [%1]==[] dotnet publish -r win-x64 -c Release --no-build
rem if "%1"=="hyperv" dotnet publish -r win-x64 -c Release --no-build /p:Mode=hyperv
rem if "%1"=="virtualbox" dotnet publish -r win-x64 -c Release --no-build /p:Mode=virtualbox
rem if "%1"=="getlinkererrors" (
rem	dotnet publish -r win-x64 -c Release --no-build /p:Mode=nolinker
rem	link /debug:full /subsystem:EFi_APPLICATION obj\x64\Release\net5.0\win-x64\native\EfiSharp.obj ..\EfiSharp.Native\x64\release\EFiSharp.Native.lib /entry:EFiMain
rem )

:end
@echo on