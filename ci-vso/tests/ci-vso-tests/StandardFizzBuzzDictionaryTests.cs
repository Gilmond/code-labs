using Ci.Vso.Lib;
using FluentAssertions;
using Xunit;

namespace Ci.Vso.Tests
{
    public class StandardFizzBuzzDictionaryTests
    {
        [Theory]
        [InlineData(1, "1")]
        [InlineData(7, "7")]
        [InlineData(11, "11")]
        [InlineData(13, "13")]
        [InlineData(31, "31")]
        [InlineData(100000, "100000")]
        public void StandardDictionaryReturnsIntegerValueForIndivisible(int value, string expected)
        {
            var dictionary = new StandardFizzBuzzDictionary("B3", "B5", "B35");
            var result = dictionary.Indivisible(value);
            result.Should().Be(expected);
        }
    }
}