#ifndef _MATH_H
#define _MATH_H

#ifdef __cplusplus
extern "C" {
#endif

typedef double double_t;
typedef float float_t;

#define NAN (__builtin_nanf(""))

// [C11/7.12.3 Classification macros]

// NOTE: fpclassify always returns exactly one of those constants
// However making them bitwise disjoint simplifies isfinite() etc.
#define FP_INFINITE  1
#define FP_NAN       2
#define FP_NORMAL    4
#define FP_SUBNORMAL 8
#define FP_ZERO      16
	
int __fpclassify(double x);
int __fpclassifyf(float x);

//Note: __fpclassifyl is not implemented currently
#define fpclassify(x) \
	(sizeof(x) == sizeof(double) ? __fpclassify(x) : \
	(sizeof(x) == sizeof(float) ? __fpclassifyf(x) : \
	(sizeof(x) == sizeof(long double) ? __fpclassifyl(x) : \
	0)))

#define isnan(x) (fpclassify(x) == FP_NAN)
	
#ifdef __cplusplus
}
#endif

#endif // _MATH_H
