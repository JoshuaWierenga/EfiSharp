@echo off

set help=F
if "%1"=="help" set help=T
if "%1"=="h" set help=T

if "%help%"=="T" (
	echo This program allows recovering from build errors and manually preparing efi images for either hyperv or virtualbox if vs fails.
	echo.
	echo Available arguments:
	echo help: Shows this text. 
	echo General building:
	echo A full path to an executable project can be used to specify which project should be built. This should be given after the command name. 
    echo If the path contains spaces, then wrap the path in quotes.
	echo VM Management:
    echo NOTE: Both hyperv and virtualbox appear to have improved support for their image being changed after adding it and so these options are
    echo most likely unnecessary.
	echo hyperv: Set permissions for the image using icacls so that an existing vm can open a rebuilt image file without manually readding it.
	echo virtualbox: Reconfigures an existing vm using VBoxManage to allow opening a rebuilt image file without manually readding it.
	echo Note that both of these options assume that the image has already been added manually to a vm and that the instructions mentioned in
	echo Properties.txt have been followed. This file is generated in the selected project folder on first use of these commands.
	echo Debug:
	echo NOTE: This only supports efi building currently
	echo getlinkererrors: Skips setting linker arguments so that a reasonable error list is shown. The normal build process shows 50+ errors on 
	echo build failure and often does not show the actual error^(s^).
	echo Miscellaneous:
	echo fixdiskimage: Unmounts a partially created disk image in case diskpart gets stuck midway and leaves it mounted with a drive letter.
	echo This should be used if "The process cannot access the file because it is being used by another process." appears after the vhd line.
	echo.
	echo By Joshua Wierenga on 28/11/2021
	
	goto :end
)

set topLevel=%~dp0
rem default project
set location=%~dp0EfiSharp\EfiSharp.csproj
rem named to avoid using projectname which is used by msbuild
set execProjectName=EfiSharp

rem get project location if in first variable
if NOT [%1]==[] (
	rem ensure potential location is not another parameter
	if NOT "%1"=="hyperv" ( if NOT "%1"=="virtualbox" ( if NOT "%1"=="getlinkererrors" if NOT "%1"=="getlinkererrorswin" ( ( if NOT "%1" == "fixdiskimage" (
			rem ensure potential location exists and refers to a file with the .csproj extension
			if exist "%1" (
				if "%~x1"==".csproj" (
					set location=%1
					set execProjectName=%~n1
				) else (
					echo "%1" is not a c# project file
					goto :end
				)
			) else (
				echo "%1" is not a valid path
				goto :end
)))))))

rem get project location if in second variable
if "%execProjectName%"=="EfiSharp" (
	if NOT [%2]==[] (
		if exist "%2" (
			if "%~x2"==".csproj" (
				set location=%2
				set execProjectName=%~n2
			) else (
				echo "%2" is not a c# project file
				goto :end
			)
		) else (
			echo "%2" is not a valid path
			goto :end
)))

echo Building %location%
cd %location%\..\

if "%1"=="fixdiskimage" msbuild /t:FixPartialVirtualDisk /p:RuntimeIdentifier=win-x64 /p:configuration=release
if "%1"=="hyperv" dotnet publish -r win-x64 -c Release /p:Platform=Efi-x64 --no-build /p:Mode=hyperv
if "%1"=="virtualbox" dotnet publish -r win-x64 -c Release /p:Platform=Efi-x64 --no-build /p:Mode=virtualbox
if "%1"=="getlinkererrors" (
	dotnet build -r win-x64 --no-self-contained -c Release /p:Platform=Efi-x64 --no-incremental /p:Mode=nolinker
	link obj\Efi-x64\Release\net6.0\win-x64\native\%execProjectName%.obj %topLevel%x64\release\EFiSharp.libc.lib /DEBUG:FULL /ENTRY:EfiMain /SUBSYSTEM:EFI_APPLICATION
)
if "%1"=="getlinkererrorswin" (
    dotnet build -r win-x64 --no-self-contained -c Release /p:Platform=Windows-x64 --no-incremental
	link obj\Windows-x64\Release\net6.0\win-x64\native\%execProjectName%.obj %topLevel%x64\release\EFiSharp.libc.lib "bcrypt.lib" "kernel32.lib" /DEBUG:FULL /ENTRY:__managed__Main /SUBSYSTEM:CONSOLE
)

:end
cd %topLevel%
@echo on