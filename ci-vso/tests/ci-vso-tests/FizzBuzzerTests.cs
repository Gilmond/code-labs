using System;
using System.Collections.Generic;
using System.Text;
using Ci.Vso.Lib;
using FluentAssertions;
using Xunit;

namespace Ci.Vso.Tests
{
    public class FizzBuzzerTests
    {
        public IFizzBuzzer GetFizzBuzzerWithStandardDictionary(string byThree, string byFive, string byThreeAndFive)
        {
            var dictionary = new StandardFizzBuzzDictionary(byThree, byFive, byThreeAndFive);

            return new FizzBuzzer(dictionary);
        }

        [Theory]
        [InlineData(0, "FizzBuzz")]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(3, "Fizz")]
        [InlineData(4, "4")]
        [InlineData(5, "Bizz")]
        [InlineData(6, "Fizz")]
        [InlineData(7, "7")]
        [InlineData(10, "Buzz")]
        [InlineData(15, "FizzBuzz")]
        [InlineData(20, "Buzz")]
        [InlineData(21, "Fizz")]
        [InlineData(30, "FizzBuzz")]
        public void FizzBuzzerProducesCorrectResultsInEnglish(int value, string expected)
        {
            var fizzBuzzer = GetFizzBuzzerWithStandardDictionary("Fizz", "Buzz", "FizzBuzz");

            var result = fizzBuzzer.Execute(value);

            result.Should().Be(expected);
        }
    }
}
