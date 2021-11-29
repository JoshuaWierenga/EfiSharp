// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga

//TODO Remove
#define DEBUG

using System;
using System.Diagnostics;

namespace Internal
{
    public static partial class Console
    {
        public static void Write(string s)
        {
            WriteCore(Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE), s);
        }

        public static partial class Error
        {
            public static void Write(string s)
            {
                //TODO Fix, currently just matching Console.EfiSharp behaviour
                //WriteCore(Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_ERROR_HANDLE), s);
                WriteCore(Interop.Kernel32.GetStdHandle(Interop.Kernel32.HandleTypes.STD_OUTPUT_HANDLE), s);
            }
        }

        private static unsafe void WriteCore(IntPtr handle, string s)
        {
            int bufferSize = s.Length * 4;
            //TODO Add Span<T>
            //Span<byte> bytes = bufferSize < 1024 ? stackalloc byte[bufferSize] : new byte[bufferSize];
            Debug.Assert(bufferSize < 1024);
            byte* pBytes = stackalloc byte[1024];
            int cbytes;

            fixed (char* pChars = s)
            {
                cbytes = Interop.Kernel32.WideCharToMultiByte(
                    Interop.Kernel32.GetConsoleOutputCP(),
                    //TODO Fix bytes
                    //0, pChars, s.Length, pBytes, bytes.Length, IntPtr.Zero, IntPtr.Zero);
                    0, pChars, s.Length, pBytes, bufferSize, IntPtr.Zero, IntPtr.Zero);
            }

            Interop.Kernel32.WriteFile(handle, pBytes, cbytes, out _, IntPtr.Zero);
        }
    }
}
