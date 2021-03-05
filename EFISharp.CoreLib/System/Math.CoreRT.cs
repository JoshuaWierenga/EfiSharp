// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*============================================================
**
** Purpose: Some floating-point math operations
**
===========================================================*/

//This class contains only static members and doesn't require serialization.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
    public static partial class Math
    {
        //TODO Add RuntimeImports.fabsf
        /*[Intrinsic]
        public static float Abs(float value)
        {
            return RuntimeImports.fabsf(value);
        }*/

        //TODO Add RuntimeImports.fabs
        /*[Intrinsic]
        public static double Abs(double value)
        {
            return RuntimeImports.fabs(value);
        }*/

        //TODO Add RuntimeImports.acos
        /*[Intrinsic]
        public static double Acos(double d)
        {
            return RuntimeImports.acos(d);
        }*/

        //TODO Add RuntimeImports.acosh
        /*[Intrinsic]
        public static double Acosh(double d)
        {
            return RuntimeImports.acosh(d);
        }*/

        //TODO Add RuntimeImports.asin
        /*[Intrinsic]
        public static double Asin(double d)
        {
            return RuntimeImports.asin(d);
        }*/

        //TODO Add RuntimeImports.asinh
        /*[Intrinsic]
        public static double Asinh(double d)
        {
            return RuntimeImports.asinh(d);
        }*/

        //TODO Add RuntimeImports.atan
        /*[Intrinsic]
        public static double Atan(double d)
        {
            return RuntimeImports.atan(d);
        }*/

        //TODO Add RuntimeImports.atan2
        /*[Intrinsic]
        public static double Atan2(double y, double x)
        {
            return RuntimeImports.atan2(y, x);
        }*/

        //TODO Add RuntimeImports.atanh
        /*[Intrinsic]
        public static double Atanh(double d)
        {
            return RuntimeImports.atanh(d);
        }*/

        //TODO Add RuntimeImports.cbrt
        /*[Intrinsic]
        public static double Cbrt(double d)
        {
            return RuntimeImports.cbrt(d);
        }*/

        //TODO Add RuntimeImports.ceil
        /*[Intrinsic]
        public static double Ceiling(double a)
        {
            return RuntimeImports.ceil(a);
        }*/

        //TODO Add RuntimeImports.cos
        /*[Intrinsic]
        public static double Cos(double d)
        {
            return RuntimeImports.cos(d);
        }*/

        //TODO Add RuntimeImports.cosh
        /*[Intrinsic]
        public static double Cosh(double value)
        {
            return RuntimeImports.cosh(value);
        }*/

        //TODO Add RuntimeImports.exp
        /*[Intrinsic]
        public static double Exp(double d)
        {
            return RuntimeImports.exp(d);
        }*/

        //TODO Add RuntimeImports.floor
        /*[Intrinsic]
        public static double Floor(double d)
        {
            return RuntimeImports.floor(d);
        }*/

        //TODO Add RuntimeImports.fma
        /*[Intrinsic]
        public static double FusedMultiplyAdd(double x, double y, double z)
        {
            return RuntimeImports.fma(x, y, z);
        }*/

        //TODO Add RuntimeImports.ilogb
        /*[Intrinsic]
        public static int ILogB(double x)
        {
            return RuntimeImports.ilogb(x);
        }*/

        //TODO Add RuntimeImports.log
        /*[Intrinsic]
        public static double Log(double d)
        {
            return RuntimeImports.log(d);
        }*/

        //TODO Add RuntimeImports.log
        /*[Intrinsic]
        public static double Log2(double x)
        {
            return RuntimeImports.log2(x);
        }*/

        //TODO Add RuntimeImports.log10
        /*[Intrinsic]
        public static double Log10(double d)
        {
            return RuntimeImports.log10(d);
        }*/

        //TODO Add RuntimeImports.pow
        /*[Intrinsic]
        public static double Pow(double x, double y)
        {
            return RuntimeImports.pow(x, y);
        }*/

        //TODO Add RuntimeImports.sin
        /*[Intrinsic]
        public static double Sin(double a)
        {
            return RuntimeImports.sin(a);
        }*/

        //TODO Add RuntimeImports.sinh
        /*[Intrinsic]
        public static double Sinh(double value)
        {
            return RuntimeImports.sinh(value);
        }*/

        //TODO Add RuntimeImports.sqrt
        /*[Intrinsic]
        public static double Sqrt(double d)
        {
            return RuntimeImports.sqrt(d);
        }*/

        //TODO Add RuntimeImports.tan
        /*[Intrinsic]
        public static double Tan(double a)
        {
            return RuntimeImports.tan(a);
        }*/

        //TODO Add RuntimeImports.tanh
        /*[Intrinsic]
        public static double Tanh(double value)
        {
            return RuntimeImports.tanh(value);
        }*/

        //TODO Add RuntimeImports.fmod
        /*[Intrinsic]
        private static double FMod(double x, double y)
        {
            return RuntimeImports.fmod(x, y);
        }*/

        //TODO Add RuntimeImports.modf
        [Intrinsic]
        private static unsafe double ModF(double x, double* intptr)
        {
            return RuntimeImports.modf(x, intptr);
        }

        //TODO Add TupleElementNamesAttribute, ValueType<U, V>, Sin and Cos
        //public static (double Sin, double Cos) SinCos(double x) => (Sin(x), Cos(x));
    }
}
