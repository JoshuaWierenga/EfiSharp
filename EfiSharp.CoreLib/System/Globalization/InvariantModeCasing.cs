// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerServices;

namespace System.Globalization
{
    internal static class InvariantModeCasing
    {
        //TODO Add CharUnicodeInfo.ToLower
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static char ToLower(char c) => CharUnicodeInfo.ToLower(c);*/

        //TODO Add CharUnicodeInfo.ToUpper
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static char ToUpper(char c) => CharUnicodeInfo.ToUpper(c);*/

        //TODO Add ReadOnlySpan<T>, Char.IsHighSurrogate, Char.IsLowSurrogate, SurrogateCasing.ToLower, String.Create and ValueTuple`2'
        /*internal static string ToLower(string s)
        {
            if (s.Length == 0)
            {
                return string.Empty;
            }

            ReadOnlySpan<char> source = s;

            int i = 0;
            while (i < s.Length)
            {
                if (char.IsHighSurrogate(source[i]) && i < s.Length - 1 && char.IsLowSurrogate(source[i + 1]))
                {
                    SurrogateCasing.ToLower(source[i], source[i + 1], out char h, out char l);
                    if (source[i] != h || source[i + 1] != l)
                    {
                        break;
                    }

                    i += 2;
                    continue;
                }

                if (ToLower(source[i]) != source[i])
                {
                    break;
                }

                i++;
            }

            if (i >= s.Length)
            {
                return s;
            }

            return string.Create(s.Length, (s, i), static (destination, state) =>
            {
                ReadOnlySpan<char> src = state.s;
                src.Slice(0, state.i).CopyTo(destination);
                InvariantModeCasing.ToLower(src.Slice(state.i), destination.Slice(state.i));
            });

        }*/

        //TODO Add ReadOnlySpan<T>, Char.IsHighSurrogate, Char.IsLowSurrogate, SurrogateCasing.ToUpper, String.Create and ValueTuple`2'
        /*internal static string ToUpper(string s)
        {
            if (s.Length == 0)
            {
                return string.Empty;
            }

            ReadOnlySpan<char> source = s;

            int i = 0;
            while (i < s.Length)
            {
                if (char.IsHighSurrogate(source[i]) && i < s.Length - 1 && char.IsLowSurrogate(source[i + 1]))
                {
                    SurrogateCasing.ToUpper(source[i], source[i + 1], out char h, out char l);
                    if (source[i] != h || source[i + 1] != l)
                    {
                        break;
                    }

                    i += 2;
                    continue;
                }

                if (ToUpper(source[i]) != source[i])
                {
                    break;
                }

                i++;
            }

            if (i >= s.Length)
            {
                return s;
            }

            return string.Create(s.Length, (s, i), static (destination, state) =>
            {
                ReadOnlySpan<char> src = state.s;
                src.Slice(0, state.i).CopyTo(destination);
                InvariantModeCasing.ToUpper(src.Slice(state.i), destination.Slice(state.i));
            });
        }*/

        //TODO Add ReadOnlySpan<T>, Span<T>, GlobalizationMode.Invariant, Char.IsHighSurrogate, Char.IsLowSurrogate, SurrogateCasing.ToUpper and ToUpper
        /*internal static void ToUpper(ReadOnlySpan<char> source, Span<char> destination)
        {
            Debug.Assert(GlobalizationMode.Invariant);
            Debug.Assert(source.Length <= destination.Length);

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                if (char.IsHighSurrogate(c) && i < source.Length - 1)
                {
                    char cl = source[i + 1];
                    if (char.IsLowSurrogate(cl))
                    {
                        // well formed surrogates
                        SurrogateCasing.ToUpper(c, cl, out char h, out char l);
                        destination[i] = h;
                        destination[i + 1] = l;
                        i++; // skip the low surrogate
                        continue;
                    }
                }

                destination[i] = ToUpper(c);
            }
        }*/

        //TODO Add ReadOnlySpan<T>, Span<T>, GlobalizationMode.Invariant, Char.IsHighSurrogate, Char.IsLowSurrogate, SurrogateCasing.ToLower and ToLower
        /*internal static void ToLower(ReadOnlySpan<char> source, Span<char> destination)
        {
            Debug.Assert(GlobalizationMode.Invariant);
            Debug.Assert(source.Length <= destination.Length);

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                if (char.IsHighSurrogate(c) && i < source.Length - 1)
                {
                    char cl = source[i + 1];
                    if (char.IsLowSurrogate(cl))
                    {
                        // well formed surrogates
                        SurrogateCasing.ToLower(c, cl, out char h, out char l);
                        destination[i] = h;
                        destination[i + 1] = l;
                        i++; // skip the low surrogate
                        continue;
                    }
                }

                destination[i] = ToLower(c);
            }
        }*/

        //TODO Add ValueTuple`2', Char.IsHighSurrogate, Char.IsLowSurrogate and UnicodeUtility.GetScalarFromUtf16SurrogatePair
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (uint, int) GetScalar(ref char source, int index, int length)
        {
            char charA = source;

            if (!char.IsHighSurrogate(charA) || index >= length - 1)
            {
                return ((uint)charA, 1);
            }

            char charB = Unsafe.Add(ref source, 1);
            if (!char.IsLowSurrogate(charB))
            {
                return ((uint)charA, 1);
            }

            return (UnicodeUtility.GetScalarFromUtf16SurrogatePair(charA, charB), 2);
        }*/

        //TODO Add GlobalizationMode.Invariant, GetScalar, CharUnicodeInfo.ToUpper
        /*internal static int CompareStringIgnoreCase(ref char strA, int lengthA, ref char strB, int lengthB)
        {
            Debug.Assert(GlobalizationMode.Invariant);

            int length = Math.Min(lengthA, lengthB);

            ref char charA = ref strA;
            ref char charB = ref strB;

            int index = 0;

            while (index < length)
            {
                (uint codePointA, int codePointLengthA) = GetScalar(ref charA, index, lengthA);
                (uint codePointB, int codePointLengthB) = GetScalar(ref charB, index, lengthB);

                if (codePointA == codePointB)
                {
                    Debug.Assert(codePointLengthA == codePointLengthB);
                    index += codePointLengthA;
                    charA = ref Unsafe.Add(ref charA, codePointLengthA);
                    charB = ref Unsafe.Add(ref charB, codePointLengthB);
                    continue;
                }

                uint aUpper = CharUnicodeInfo.ToUpper(codePointA);
                uint bUpper = CharUnicodeInfo.ToUpper(codePointB);

                if (aUpper == bUpper)
                {
                    Debug.Assert(codePointLengthA == codePointLengthB);
                    index += codePointLengthA;
                    charA = ref Unsafe.Add(ref charA, codePointLengthA);
                    charB = ref Unsafe.Add(ref charB, codePointLengthB);
                    continue;
                }

                return (int)codePointA - (int)codePointB;
            }

            return lengthA - lengthB;
        }*/

        //TODO Add ReadOnlySpan<T>, GlobalizationMode.Invariant, &MemoryMarshal.GetReference, Char.IsHighSurrogate, ToUpper, Char.IsLowSurrogate and SurrogateCasing.Equal
        /*internal static unsafe int IndexOfIgnoreCase(ReadOnlySpan<char> source, ReadOnlySpan<char> value)
        {
            Debug.Assert(value.Length > 0);
            Debug.Assert(value.Length <= source.Length);
            Debug.Assert(GlobalizationMode.Invariant);

            fixed (char* pSource = &MemoryMarshal.GetReference(source))
            fixed (char* pValue = &MemoryMarshal.GetReference(value))
            {
                char* pSourceLimit = pSource + (source.Length - value.Length);
                char* pValueLimit = pValue + value.Length - 1;
                char* pCurrentSource = pSource;

                while (pCurrentSource <= pSourceLimit)
                {
                    char* pVal = pValue;
                    char* pSrc = pCurrentSource;

                    while (pVal <= pValueLimit)
                    {
                        if (!char.IsHighSurrogate(*pVal) || pVal == pValueLimit)
                        {
                            if (*pVal != *pSrc && ToUpper(*pVal) != ToUpper(*pSrc))
                                break; // no match

                            pVal++;
                            pSrc++;
                            continue;
                        }

                        if (char.IsHighSurrogate(*pSrc) && char.IsLowSurrogate(*(pSrc + 1)) && char.IsLowSurrogate(*(pVal + 1)))
                        {
                            // Well formed surrogates
                            // both the source and the Value have well-formed surrogates.
                            if (!SurrogateCasing.Equal(*pSrc, *(pSrc + 1), *pVal, *(pVal + 1)))
                                break; // no match

                            pSrc += 2;
                            pVal += 2;
                            continue;
                        }

                        if (*pVal != *pSrc)
                            break; // no match

                        pSrc++;
                        pVal++;
                    }

                    if (pVal > pValueLimit)
                    {
                        // Found match.
                        return (int)(pCurrentSource - pSource);
                    }

                    pCurrentSource++;
                }

                return -1;
            }
        }
        
        internal static unsafe int LastIndexOfIgnoreCase(ReadOnlySpan<char> source, ReadOnlySpan<char> value)
        {
            Debug.Assert(value.Length > 0);
            Debug.Assert(value.Length <= source.Length);
            Debug.Assert(GlobalizationMode.Invariant);

            fixed (char* pSource = &MemoryMarshal.GetReference(source))
            fixed (char* pValue = &MemoryMarshal.GetReference(value))
            {
                char* pValueLimit = pValue + value.Length - 1;
                char* pCurrentSource = pSource + (source.Length - value.Length);

                while (pCurrentSource >= pSource)
                {
                    char* pVal = pValue;
                    char* pSrc = pCurrentSource;

                    while (pVal <= pValueLimit)
                    {
                        if (!char.IsHighSurrogate(*pVal) || pVal == pValueLimit)
                        {
                            if (*pVal != *pSrc && ToUpper(*pVal) != ToUpper(*pSrc))
                                break; // no match

                            pVal++;
                            pSrc++;
                            continue;
                        }

                        if (char.IsHighSurrogate(*pSrc) && char.IsLowSurrogate(*(pSrc + 1)) && char.IsLowSurrogate(*(pVal + 1)))
                        {
                            // Well formed surrogates
                            // both the source and the Value have well-formed surrogates.
                            if (!SurrogateCasing.Equal(*pSrc, *(pSrc + 1), *pVal, *(pVal + 1)))
                                break; // no match

                            pSrc += 2;
                            pVal += 2;
                            continue;
                        }

                        if (*pVal != *pSrc)
                            break; // no match

                        pSrc++;
                        pVal++;
                    }

                    if (pVal > pValueLimit)
                    {
                        // Found match.
                        return (int)(pCurrentSource - pSource);
                    }

                    pCurrentSource--;
                }

                return -1;
            }
        }*/
    }
}