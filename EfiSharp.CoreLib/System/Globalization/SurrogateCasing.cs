﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Globalization
{
    internal static class SurrogateCasing
    {
        //TODO Add Char.IsHighSurrogate, Char.IsLowSurrogate, CharUnicodeInfo.ToUpper and UnicodeUtility.GetScalarFromUtf16SurrogatePair
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ToUpper(char h, char l, out char hr, out char lr)
        {
            Debug.Assert(char.IsHighSurrogate(h));
            Debug.Assert(char.IsLowSurrogate(l));

            UnicodeUtility.GetUtf16SurrogatesFromSupplementaryPlaneScalar(CharUnicodeInfo.ToUpper(UnicodeUtility.GetScalarFromUtf16SurrogatePair(h, l)), out hr, out lr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ToLower(char h, char l, out char hr, out char lr)
        {
            Debug.Assert(char.IsHighSurrogate(h));
            Debug.Assert(char.IsLowSurrogate(l));

            UnicodeUtility.GetUtf16SurrogatesFromSupplementaryPlaneScalar(CharUnicodeInfo.ToLower(UnicodeUtility.GetScalarFromUtf16SurrogatePair(h, l)), out hr, out lr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool Equal(char h1, char l1, char h2, char l2)
        {
            ToUpper(h1, l1, out char hr1, out char lr1);
            ToUpper(h2, l2, out char hr2, out char lr2);

            return hr1 == hr2 && lr1 == lr2;
        }*/
    }
}
