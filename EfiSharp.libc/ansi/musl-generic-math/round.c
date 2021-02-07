#include "libm.h"

//NOTE: these epsilons did not originally have __ on each side but that gave "use of undeclared identifier"
#if FLT_EVAL_METHOD==0 || FLT_EVAL_METHOD==1
#define EPS __DBL_EPSILON__
#elif FLT_EVAL_METHOD==2
#define EPS __LDBL_EPSILON__
#endif
static const double_t toint = 1 / EPS;

double round(double x)
{
	union { double f; uint64_t i; } u = { x };
	int e = u.i >> 52 & 0x7ff;
	double_t y;

	if (e >= 0x3ff + 52)
		return x;
	if (u.i >> 63)
		x = -x;
	if (e < 0x3ff - 1) {
		/* raise inexact if x!=0 */
		FORCE_EVAL(x + toint);
		return 0 * u.f;
	}
	y = x + toint - toint - x;
	if (y > 0.5)
		y = y + x - 1;
	else if (y <= -0.5)
		y = y + x + 1;
	else
		y = y + x;
	if (u.i >> 63)
		y = -y;
	return y;
}
