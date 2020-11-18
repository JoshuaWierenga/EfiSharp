@echo off
dotnet build -r win-x64 -c Release --no-incremental

if [%1]==[] dotnet publish -r win-x64 -c Release --no-build
if "%1"=="hyperv" dotnet publish -r win-x64 -c Release --no-build /p:Mode=hyperv
if "%1"=="virtualbox" dotnet publish -r win-x64 -c Release --no-build /p:Mode=virtualbox
@echo on