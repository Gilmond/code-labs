namespace Ci.Vso.Lib
{
    public class FizzBuzzer : IFizzBuzzer
    {
        private readonly IFizzBuzzDictionary _fizzBuzzDictionary;

        public FizzBuzzer(IFizzBuzzDictionary fizzBuzzDictionary)
        {
            _fizzBuzzDictionary = fizzBuzzDictionary;
        }

        public string Execute(int value)
        {
            var div3 = value % 3 == 0;
            var div5 = value % 5 == 0;

            if (div3 && div5) return _fizzBuzzDictionary.DivisibleByThreeAndFive(value);
            if (div3) return _fizzBuzzDictionary.DivisibleByThree(value);
            if (div5) return _fizzBuzzDictionary.DivisibleByFive(value);
            return _fizzBuzzDictionary.Indivisible(value);
        }
    }
}
