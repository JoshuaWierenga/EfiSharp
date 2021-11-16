@echo off

set help=F
if "%1"=="help" set help=T
if "%1"=="h" set help=T

if "%help%"=="T" (
	echo This program allows building EfiSharp and also preparing the built image for either hyperv or virtualbox.
	echo.
	echo Available arguments:
	echo help: Shows this text. 
	echo General building:
	echo A full path to an executable project can be used to specify which project should be built. This can also be done with the vm management
	echo options in which case the path should be given after the vm option. If the path contains spaces then wrap the path in quotes.
	echo VM Management:
	echo hyperv: Set permissions for the image using icacls so that an existing vm can open a rebuilt image file without manually readding it.
	echo virtualbox: Reconfigures an existing vm using VBoxManage to allow opening a rebuilt image file without manually readding it.
	echo Note that both of these options assume that the image has already been added manually to a vm and that the instructions mentioned in
	echo Properties.txt which is generated in the project folder on first build have been followed.
	echo Debug:
	echo NOTE: Untested with new build system
	echo getlinkererrors: Skips setting linker arguments so that a reasonable error list is shown. The normal build process shows 50+ errors on 
	echo build failure and often does not show the actual error^(s^).
	echo Miscellaneous:
	echo fixdiskimage: Unmounts a partially created disk image in case diskpart gets stuck midway and leaves it mounted with a drive letter.
	echo This should be used if "The process cannot access the file because it is being used by another process." appears after the vhd line.
	echo.
	echo By Joshua Wierenga on 9/02/2021
	
	goto :end
)

set topLevel=%~dp0
rem default project
set location=%~dp0EfiSharp\EfiSharp.csproj
rem named to avoid using projectname which is used by msbuild
set execProjectName=EfiSharp
set defaultBuild=F

rem get project location if in first variable
if NOT [%1]==[] (
	rem ensure potential location is not another parameter
	if NOT "%1"=="hyperv" ( if NOT "%1"=="virtualbox" ( if NOT "%1"=="getlinkererrors" ( if NOT "%1" == "fixdiskimage" ( if NOT "%1" == "wintest" (
			rem ensure potential location exists and refers to a file with the .csproj extension
			if exist "%1" (
				if "%~x1"==".csproj" (
					find "<Import Project=""$(DirectoryBuildPropsPath)\..\EfiExe.Build.props"" />" "%1" > nul
					if errorlevel 1 (
						echo "%1" is not configured as a efi executable
						goto :end
					)
					set location=%1
					set execProjectName=%~n1
					set defaultBuild=T
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
				find "<Import Project=""$(DirectoryBuildPropsPath)\..\EfiExe.Build.props"" />" "%2" > nul
				if errorlevel 1 (
					echo "%2" is not configured as a efi executable
					goto :end
				)
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

if "%1"=="fixdiskimage" (
	msbuild /t:FixPartialVirtualDisk /p:RuntimeIdentifier=win-x64 /p:configuration=release
	goto :end
)

rem Compilation of EfiSharp.CoreLib, EfiSharp.Console and the specified project to make a dll file containing il
if "%1" == "wintest" ( 
    dotnet build -r win-x64 --no-self-contained -c Release --no-incremental
) else (
    dotnet build -r win-x64 --no-self-contained -c Efi-Release --no-incremental
)
if errorlevel 1 (
	goto :end
)

rem EFiSharp.libc compliation to make EFiSharp.libc.lib
msbuild %topLevel%EfiSharp.libc\EFiSharp.libc.vcxproj /p:configuration=release
if errorlevel 1 (
	goto :end
)

if [%1]==[] dotnet publish -r win-x64 -c Efi-Release --no-build
if "%defaultBuild%"=="T" dotnet publish -r win-x64 -c Efi-Release --no-build
if "%1"=="hyperv" dotnet publish -r win-x64 -c Efi-Release --no-build /p:Mode=hyperv
if "%1"=="virtualbox" dotnet publish -r win-x64 -c Efi-Release --no-build /p:Mode=virtualbox
if "%1"=="wintest" (
    dotnet publish -r win-x64 -c Release --no-build /p:Mode=nolinker
    link obj\x64\Release\net6.0\win-x64\native\%execProjectName%.obj %topLevel%EfiSharp.libc\x64\release\EFiSharp.libc.lib /DEBUG:FULL /ENTRY:__managed__Main /SUBSYSTEM:CONSOLE "bcrypt.lib" "kernel32.lib" "user32.lib" "C:\Users\Joshua Wierenga\.nuget\packages\runtime.win-x64.microsoft.dotnet.ilcompiler\7.0.0-alpha.1.21562.1\sdk\Runtime.WorkstationGC.lib" /OUT:"bin\x64\Release\net6.0\win-x64\native\EfiSharp.exe"
)
rem TODO Support getlinkererrors with wintest, same linker arg but without Runtime.Workstation.GC.lib
if "%1"=="getlinkererrors" (
	dotnet publish -r win-x64 -c Efi-Release --no-build /p:Mode=nolinker
	link obj\x64\Efi-Release\net6.0\win-x64\native\%execProjectName%.obj %topLevel%EfiSharp.libc\x64\release\EFiSharp.libc.lib /DEBUG:FULL /ENTRY:EfiMain /SUBSYSTEM:EFI_APPLICATION
)

:end
cd %topLevel%
@echo on