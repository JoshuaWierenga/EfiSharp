//From https://raw.githubusercontent.com/dotnet/samples/a4cfd58/core/console-apps/fibonacci-msbuild/Program.cs
// Changes made by Joshua Wierenga.

using System;
using System.Runtime;

namespace Hello
{
    class Program
    {
        [RuntimeExport("Main")]
        static void Main()
        {
            //if (args.Length > 0)
            {
                //Console.WriteLine($"Hello {args[0]}!");
            }
            //else
            {
                Console.WriteLine("Hello!");
            }

            Console.WriteLine("Fibonacci Numbers 1-15:");

            for (int i = 0; i < 15; i++)
            {
                //Console.WriteLine($"{i + 1}: {FibonacciNumber(i)}");
                Console.Write(i + 1);
                Console.Write(": ");
                Console.WriteLine(FibonacciNumber(i));
            }
        }

        static int FibonacciNumber(int n)
        {
            int a = 0;
            int b = 1;
            int tmp;

            for (int i = 0; i < n; i++)
            {
                tmp = a;
                a = b;
                b += tmp;
            }

            return a;
        }
    }
}