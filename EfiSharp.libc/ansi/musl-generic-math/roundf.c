#include "libm.h"

//NOTE: these epsilons did not originally have __ on each side but that gave "use of undeclared identifier"
#if FLT_EVAL_METHOD==0
#define EPS __FLT_EPSILON__
#elif FLT_EVAL_METHOD==1
#define EPS __DBL_EPSILON__
#elif FLT_EVAL_METHOD==2
#define EPS __LDBL_EPSILON__
#endif
static const float_t toint = 1 / EPS;

float roundf(float x)
{
	union { float f; uint32_t i; } u = { x };
	int e = u.i >> 23 & 0xff;
	float_t y;

	if (e >= 0x7f + 23)
		return x;
	if (u.i >> 31)
		x = -x;
	if (e < 0x7f - 1) {
		FORCE_EVAL(x + toint);
		return 0 * u.f;
	}
	y = x + toint - toint - x;
	if (y > 0.5f)
		y = y + x - 1;
	else if (y <= -0.5f)
		y = y + x + 1;
	else
		y = y + x;
	if (u.i >> 31)
		y = -y;
	return y;
}