#ifndef _CLASSIFY_H
#define _CLASSIFY_H

#include "../ansi/include/math.h"

#define MS_FP_INFINITE  1
#define MS_FP_NAN       2
#define MS_FP_NORMAL    (-1)
#define MS_FP_SUBNORMAL (-2)
#define MS_FP_ZERO      0

//Wrapper for fpclassify macro to use microsoft floating point type definitions
#define CLASSIFY(x) \
	(fpclassify(x) == FP_INFINITE ? MS_FP_INFINITE :   \
	(fpclassify(x) == FP_NAN ? MS_FP_NAN :             \
	(fpclassify(x) == FP_NORMAL ? MS_FP_NORMAL :       \
	(fpclassify(x) == FP_SUBNORMAL ? MS_FP_SUBNORMAL : \
	MS_FP_ZERO))))

#endif
