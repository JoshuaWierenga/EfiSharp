#ifndef _MLIBC_STDINT_H
#define _MLIBC_STDINT_H

#include "bits/types.h"

// ----------------------------------------------------------------------------
// Type definitions.
// ----------------------------------------------------------------------------

// Fixed-width (signed).
typedef __mlibc_int32 int32_t;
typedef __mlibc_int64 int64_t;

// Fixed-width (unsigned).
typedef __mlibc_uint8  uint8_t;
typedef __mlibc_uint16 uint16_t;
typedef __mlibc_uint32 uint32_t;
typedef __mlibc_uint64 uint64_t;

// Miscellaneous (signed).
typedef __mlibc_intptr intptr_t;

// Miscellaneous (unsigned).
typedef __mlibc_uintptr uintptr_t;

#endif // _MLIBC_STDINT_H
