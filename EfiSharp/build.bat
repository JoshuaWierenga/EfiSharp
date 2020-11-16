@echo off
dotnet build -r win-x64 -c Release --no-incremental
dotnet publish -r win-x64 -c Release --no-build
@echo on