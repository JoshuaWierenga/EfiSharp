# CMakeList.txt : CMake project for EfiSharp.Native
cmake_minimum_required (VERSION 3.8)

add_compile_definitions(FEATURE_CACHED_INTERFACE_DISPATCH;TARGET_AMD64;HOST_64BIT;HOST_AMD64)

set(local 
"Runtime/amd64/AsmMacros.inc"
"Runtime/amd64/StubDispatch.asm"
"Runtime/amd64/UniversalTransition.asm"
"Runtime/inc/type_traits.hpp"
"Runtime/inc/TargetPtrs.h"
"Runtime/inc/rhbinder.h"
"Runtime/inc/daccess.h"
"Runtime/uefi/PalRedhawkInline.h"
"Runtime/CachedInterfaceDispatch.cpp"
"Runtime/CachedInterfaceDispatch.h"
"Runtime/CommonMacros.h"
"Runtime/rhassert.h"
)
set(nativeaot )

# Create EfiSharp.Native
add_library (EfiSharp.Native "${local}" "${nativeaot}")

target_link_libraries(EfiSharp.Native PUBLIC "../EfiSharp.libc/x64/release/EFiSharp.libc.lib")

#This is stupid, add_compile_definitions should work but is still broken for ASM_MASM
#https://gitlab.kitware.com/cmake/cmake/-/issues/19178
set_source_files_properties("Runtime/amd64/StubDispatch.asm" TARGET_DIRECTORY EfiSharp.Native PROPERTIES COMPILE_DEFINITIONS FEATURE_CACHED_INTERFACE_DISPATCH)
set_source_files_properties("Runtime/amd64/UniversalTransition.asm" TARGET_DIRECTORY EfiSharp.Native PROPERTIES COMPILE_DEFINITIONS FEATURE_DYNAMIC_CODE)