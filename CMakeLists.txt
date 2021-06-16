# CMakeList.txt : Top-level CMake project file for EfiSharp, currently only handles EfiSharp.Native
cmake_minimum_required (VERSION 3.8)

project ("EFISharp" CXX ASM_MASM)

# Include sub-projects.
add_subdirectory ("EfiSharp.Native")