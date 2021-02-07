#include "../ansi/include/math.h"

short __cdecl _dclass(double x)
{
	return __fpclassify(x);
}