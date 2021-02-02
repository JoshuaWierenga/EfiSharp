using System;

namespace FloatWriteDebug
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("float Output Test:");
            Console.WriteLine("  Actual   | CoreCLR/NativeAot |  EfiSharp  | EfiSharp Zero Pad");
            Console.Write("3.14159E+4 | ");
            Console.Write($"{3.14159E+4f}".PadRight(18) + "| ");
            Write(3.14159E+4f);
            Console.Write(" | ");
            WriteHm(3.14159E+4f);
            Console.WriteLine();

            Console.Write("-9.999999".PadRight(11) + "| ");
            Console.Write($"{-9.999999f}".PadRight(18) + "| ");
            Write(-9.999999f);
            Console.Write("| ");
            WriteHm(-9.999999f);
            Console.WriteLine();

            Console.Write("3.1".PadRight(11) + "| ");
            Console.Write($"{3.1f}".PadRight(18) + "| ");
            Write(3.1f);
            Console.Write(" | ");
            WriteHm(3.1f);
            Console.WriteLine();

            Console.Write("177.004".PadRight(11) + "| ");
            Console.Write($"{177.004f}".PadRight(18) + "| ");
            Write(177.004f);
            Console.Write(" | ");
            WriteHm(177.004f);
            Console.WriteLine();

            Console.Write("-14999.004".PadRight(11) + "| ");
            Console.Write($"{-14999.004f}".PadRight(18) + "| ");
            Write(-14999.004f);
            Console.Write("| ");
            WriteHm(-14999.004f);
            Console.WriteLine();

            Console.Write("-1234.5678".PadRight(11) + "| ");
            Console.Write($"{-1234.5678f}".PadRight(18) + "| ");
            Write(-1234.5678f);
            Console.Write("| ");
            WriteHm(-1234.5678f);
            Console.WriteLine();

            Console.Write("1.2345678".PadRight(11) + "| ");
            Console.Write($"{1.2345678f}".PadRight(18) + "| ");
            Write(1.2345678f);
            Console.Write(" | ");
            WriteHm(1.2345678f);
            Console.WriteLine();

            Console.Write("8.765432".PadRight(11) + "| ");
            Console.Write($"{8.765432f}".PadRight(18) + "| ");
            Write(8.765432f);
            Console.Write(" | ");
            WriteHm(8.765432f);
            Console.WriteLine();

            Console.Write("18.0009".PadRight(11) + "| ");
            Console.Write($"{18.0009f}".PadRight(18) + "| ");
            Write(18.0009f);
            Console.Write(" | ");
            WriteHm(18.0009f);
            Console.WriteLine();

            Console.Write("18.00009".PadRight(11) + "| ");
            Console.Write($"{18.00009f}".PadRight(18) + "| ");
            Write(18.00009f);
            Console.Write(" | ");
            WriteHm(18.00009f);
            Console.WriteLine();

            Console.Write("-141.0001".PadRight(11) + "| ");
            Console.Write($"{-141.0001f}".PadRight(18) + "| ");
            Write(-141.0001f);
            Console.Write(" | ");
            WriteHm(-141.0001f);
            Console.WriteLine();
        }

        //Modified ulong print from EfiSharp, uses Console.Write to print the char* and returns the actual length of the printing ulong
        private static int Write(ulong value, int decimalLength)
        {
            char* pValue = stackalloc char[decimalLength + 1];
            sbyte digitPosition = (sbyte)(decimalLength - 1); //This is designed to go negative for numbers with decimalLength digits

            do
            {
                pValue[digitPosition--] = (char)(value % 10 + '0');
                value /= 10;
            } while (value > 0);


            Console.Write(new string(&pValue[digitPosition + 1]));
            //UefiApplication.Out->OutputString(&pValue[digitPosition + 1]);

            //actual length of integer in terms of decimal digits
            return decimalLength - 1 - digitPosition;
        }

        //Labeled As EfiSharp in testing
        public static void Write(float value)
        {
            if (value < 0)
            {
                Console.Write('-');
                value = -value;
            }

            //Print integer component of float
            //TODO Check if iLength will be inaccurate if (ulong)value == 0 or 1
            //9 is used since at a maximum, a float can store that many digits
            int iLength = Write((ulong)value, 9);
            int fLength = 9 - iLength;


            //Print decimal component of float
            Console.Write('.');

            //Test for zeros after the decimal point followed by more numbers, if found, pValue will be printed which is a less accurate method that can handle that
            if ((uint)((value - (ulong)value) * 10) == 0)
            {
                char* pValue = stackalloc char[fLength + 1];
                value -= (ulong)value;
                for (int i = 0; i < fLength; i++)
                {
                    value *= 10;
                    pValue[i] = (char)((uint)value % 10 + '0');
                }

                Console.Write(new string(pValue));
                return;
            }

            //This method is more accurate since it avoids repeated multiplication of number but loses zeros at the front of the decimal
            int tenPower = 10;
            for (int i = 0; i < fLength - 1; i++)
            {
                tenPower *= 10;
            }

            //Retrieve remaining decimal component of mantissa as integer
            uint fPart = (uint)((value - (uint)value) * tenPower);
            //uint fPart2 = (uint)(value * tenPower - (uint)value * tenPower);

            Write(fPart, fLength);
        }

        //Labeled As EfiSharp Zero Pad in testing
        public static void WriteHm(float value)
        {
            if (value < 0)
            {
                Console.Write('-');
                value = -value;
            }

            //Print integer component of float
            //TODO Check if iLength will be inaccurate if (ulong)value == 0 or 1
            //9 is used since at a maximum, a float can store that many digits
            int iLength = Write((ulong)value, 9);
            int fLength = 9 - iLength;

            //Print decimal component of float
            Console.Write('.');

            //Test for zeros after the decimal point followed by more numbers, if found, a slightly modified algorithm will be used
            int leadingZeroCount = 0;
            uint firstNonZeroDigit = 0;
            if ((uint)((value - (ulong)value) * 10) == 0)
            {
                float valueTest = value - (ulong)value;
                for (int i = 0; i < fLength; i++)
                {
                    valueTest *= 10;

                    if ((uint)valueTest % 10 == 0)
                    {
                        leadingZeroCount++;
                    }
                    else
                    {
                        firstNonZeroDigit = (uint)valueTest % 10;
                        break;
                    }
                }
            }

            int tenPower = 10;
            for (int i = 0; i < fLength - 1; i++)
            {
                tenPower *= 10;
            }

            uint fPart;
            //Retrieve remaining decimal component of mantissa as integer
            if (leadingZeroCount != 0)
            {
                //This result is less accurate than the one below in general but appears more accurate in cases with leading zeros in the decimal part of the float
                fPart = (uint)(value * tenPower - (uint)value * tenPower);

                //NOTE: This was wrong, x.(00...)1 could be stored any number of ways depending on the number and so this fix cannot account for that which breaks the result in some cases, see -141.0001 test
                //floats cannot store x.(00...)1 correctly and appear to instead store x.(00... + 0)9999999, this version of the algorithm appears to account for this and so we don't want to add the extra zero
                if (firstNonZeroDigit == 1)
                {
                    leadingZeroCount--;
                }
                for (int i = 0; i < leadingZeroCount; i++)
                {
                    Console.Write('0');
                }
            }
            else
            {
                fPart = (uint)((value - (uint)value) * tenPower);
            }

            Write(fPart, fLength);
        }
    }
}
