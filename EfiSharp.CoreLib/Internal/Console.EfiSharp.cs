// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using EfiSharp;

namespace Internal
{
    public static partial class Console
    {
        public static unsafe void Write(string s)
        {
            UefiApplication.Out->OutputString(s);
        }

        public static partial class Error
        {
            public static unsafe void Write(string s)
            {
                //I looked in to stderr support but at least with VMs I can't find a way to view the output
                nuint currentColours = (nuint)UefiApplication.Out->Mode->Attribute;
                //Set foreground colour to red and background colour to black
                UefiApplication.Out->SetAttribute(12);
                UefiApplication.Out->OutputString(s);
                UefiApplication.Out->SetAttribute(currentColours);
            }
        }

        private static unsafe char ReadKey()
        {
            if (UefiApplication.In->ReadKeyStrokeEx(out EFI_KEY_DATA input) != EFI_STATUS.EFI_SUCCESS)
            {
                UefiApplication.SystemTable->BootServices->WaitForEvent(UefiApplication.In->WaitForKeyEx);
                UefiApplication.In->ReadKeyStrokeEx(out input);
            }

            Write(input.Key.UnicodeChar.ToString());

            return input.Key.UnicodeChar;
        }

        //TODO I tried using {U}Int32.ToString but that causes the program to hang for 0, that needs to be fixed
        private static unsafe void WritePositiveInt(uint value)
        {
            //It would be possible to use char[] here but that requires freeing afterwards unlike stack allocations where are removed automatically
            char* pValue = stackalloc char[11];
            sbyte digitPosition = 9; //This is designed to go negative for numbers with 10 digits

            do
            {
                pValue[digitPosition--] = (char)(value % 10 + '0');
                value /= 10;
            } while (value > 0);

            UefiApplication.Out->OutputString(&pValue[digitPosition + 1]);
        }

        internal static unsafe void ConsoleSetup()
        {
            //TODO To match dotnet behavior the cursor should blink, 500ms timer interrupt?
            UefiApplication.Out->EnableCursor(true);

            //Prevent console from blacking out until ClearScreen is called later on, the need for this appears to change from build to build
            UefiApplication.Out->ClearScreen();

            UefiApplication.Out->QueryMode((nuint)UefiApplication.Out->Mode->Mode, out nuint currentBufferWidth, out nuint currentBufferHeight);
            uint modeCount = (uint)UefiApplication.Out->Mode->MaxMode;
            
            Write("Current Mode: ");
            WritePositiveInt((uint)UefiApplication.Out->Mode->Mode);
            Write("\r\nCurrent Size: ");
            Write("(");
            Write(((uint)currentBufferWidth).ToString());
            Write(", ");
            Write(((uint)currentBufferHeight).ToString());
            WriteLine(")");

            WriteLine("\r\nPlease select console size");
            Write("Supported modes: ");
            for (uint i = 0; i < modeCount; i++)
            {
                UefiApplication.Out->QueryMode(i, out nuint cols, out nuint rows);

                Write("\r\nMode ");
                WritePositiveInt(i);
                Write(" Size: ");
                Write("(");
                WritePositiveInt((uint)cols);
                Write(", ");
                WritePositiveInt((uint)rows);
                Write(")");
            }

            nuint selectedMode = modeCount;
            while (selectedMode >= modeCount)
            {
                Write("\r\nSelect Mode: ");
                selectedMode = (nuint)ReadKey() - 0x30;
            }

            UefiApplication.Out->SetMode(selectedMode);
            Write("\r\nNew Mode: ");
            //TODO Figure out why this needs WritePositiveInt regardless of value? While in general 0 could be the biggest size, its 3 in hyperv and that is what I use
            WritePositiveInt((uint)UefiApplication.Out->Mode->Mode);

            WriteLine("\r\nPress any key to continue");
            ReadKey();
            UefiApplication.Out->ClearScreen();
        }
    }
}
