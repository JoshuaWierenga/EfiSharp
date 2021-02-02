using System;
using System.Runtime;

namespace BranchesAndLoops
{
    class Program
    {
        static void ExploreIf()
        {
            int a = 5;
            int b = 3;
            if (a + b > 10)
            {
                Console.WriteLine("The answer is greater than 10");
            }
            else
            {
                Console.WriteLine("The answer is not greater than 10");
            }

            int c = 4;
            if ((a + b + c > 10) && (a > b))
            {
                Console.WriteLine("The answer is greater than 10");
                Console.WriteLine("And the first number is greater than the second");
            }
            else
            {
                Console.WriteLine("The answer is not greater than 10");
                Console.WriteLine("Or the first number is not greater than the second");
            }

            if ((a + b + c > 10) || (a > b))
            {
                Console.WriteLine("The answer is greater than 10");
                Console.WriteLine("Or the first number is greater than the second");
            }
            else
            {
                Console.WriteLine("The answer is not greater than 10");
                Console.WriteLine("And the first number is not greater than the second");
            }
        }

        static void ChallengeAnswer()
        {
            int sum = 0;
            for (int number = 1; number < 21; number++)
            {
                if (number % 3 == 0)
                {
                    sum = sum + number;
                }
            }
            //Console.WriteLine($"The sum is {sum}");
            Console.Write("The sum is ");
            Console.WriteLine(sum);
        }

        [RuntimeExport("Main")]
        static void Main()
        {
            ExploreIf();

            //Added because of limited console size
            Console.WriteLine("\nPress any key to continue.");
            Console.Read();

            int counter = 0;
            while (counter < 10)
            {
                //Console.WriteLine($"Hello World! The counter is {counter}");
                Console.Write("Hello World! The counter is ");
                Console.WriteLine(counter);
                counter++;
            }

            //Added because of limited console size
            Console.WriteLine("\nPress any key to continue.");
            Console.Read();

            counter = 0;
            do
            {
                //Console.WriteLine($"Hello World! The counter is {counter}");
                Console.Write("Hello World! The counter is ");
                Console.WriteLine(counter);
                counter++;
            } while (counter < 10);

            //Added because of limited console size
            Console.WriteLine("\nPress any key to continue.");
            Console.Read();

            for (int index = 0; index < 10; index++)
            {
                //Console.WriteLine($"Hello World! The index is {index}");
                Console.Write("Hello World! The index is ");
                Console.WriteLine(index);
            }

            //Added because of limited console size
            Console.WriteLine("\nPress any key to continue.");
            Console.Read();

            ChallengeAnswer();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
