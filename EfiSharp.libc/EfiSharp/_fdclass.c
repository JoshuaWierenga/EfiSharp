#include "../ansi/include/math.h"

short __cdecl _fdclass(float x)
{
	return __fpclassifyf(x);
}