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
	echo hyperv: Set permissions for the image using icacls so that an existing vm can open a rebuilt image file without manually readding it.
	echo virtualbox: Reconfigures an existing vm using VBoxManage to allow opening a rebuilt image file without manually readding it.
	echo Note that both of these options assume that the image which is stored in EfiSharp\bin\x64\Release\net5.0\win-x64\native\EfiSharp.vhd has 
	echo already been added manually to a vm and that the instructions mentioned at the bottom of EfiSharp.csproj have be followed.
	echo Debug:
	echo getlinkererrors: Skips setting linker arguments so that a reasonable error list is shown. The normal build process shows 50+ errors on 
	echo build failure and often does not show the actual error^(s^).
	echo.
	echo By Joshua Wierenga on 3/02/2021
	
	goto :end
)

dotnet build -r win-x64 -c Release --no-incremental
msbuild ..\EfiSharp.Native\EFiSharp.Native.vcxproj /p:configuration=release

if [%1]==[] dotnet publish -r win-x64 -c Release --no-build
if "%1"=="hyperv" dotnet publish -r win-x64 -c Release --no-build /p:Mode=hyperv
if "%1"=="virtualbox" dotnet publish -r win-x64 -c Release --no-build /p:Mode=virtualbox
if "%1"=="getlinkererrors" (
	dotnet publish -r win-x64 -c Release --no-build /p:Mode=nolinker
	link /debug:full /subsystem:EFi_APPLICATION obj\x64\Release\net5.0\win-x64\native\EfiSharp.obj ..\EfiSharp.Native\x64\release\EFiSharp.Native.lib /entry:EFiMain
)

:end
@echo on