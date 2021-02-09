/* origin: FreeBSD /usr/src/lib/msun/src/math_private.h */
/*
 * ====================================================
 * Copyright (C) 1993 by Sun Microsystems, Inc. All rights reserved.
 *
 * Developed at SunPro, a Sun Microsystems, Inc. business.
 * Permission to use, copy, modify, and distribute this
 * software is freely granted, provided that this notice
 * is preserved.
 * ====================================================
 */


#ifndef _LIBM_H
#define _LIBM_H

#include "../include/math.h"
#include "../../internal/include/stdint.h"

#define FORCE_EVAL(x) do {                    \
	if (sizeof(x) == sizeof(float)) {         \
		volatile float __x;                   \
		__x = (x);							  \
	} else if (sizeof(x) == sizeof(double)) { \
		volatile double __x;				  \
		__x = (x);							  \
	} else {                                  \
		volatile long double __x;			  \
		__x = (x);							  \
	}                                         \
} while(0)

#endif
