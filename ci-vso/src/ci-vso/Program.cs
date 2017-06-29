using System;
using Ci.Vso.Lib;

namespace Ci.Vso
{
    class Program
    {
        static void Main(string[] args)
        {
            var fizzBuzzer = new FizzBuzzer(new StandardFizzBuzzDictionary("Fizz", "Buzz", "FizzBuzz"));

            for (var i = 1; i <= 10000; i++)
            {
                var result = fizzBuzzer.Execute(i);
                
                Console.WriteLine($"{i} = {result}");
            }

            Console.WriteLine("Fin!");
            var r = Console.Read();
        }
    }
}