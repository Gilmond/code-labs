using System;

namespace Ci.Vso.Lib
{
    public class StandardFizzBuzzDictionary : IFizzBuzzDictionary
    {
        private readonly string _byThree;
        private readonly string _byFive;
        private readonly string _byThreeAndFive;

        public StandardFizzBuzzDictionary(string byThree, string byFive, string byThreeAndFive)
        {
            _byThree = byThree;
            _byFive = byFive;
            _byThreeAndFive = byThreeAndFive;
        }

        public Func<int, string> DivisibleByThree => (i) => _byThree;
        public Func<int, string> DivisibleByFive => (i) => _byFive;
        public Func<int, string> DivisibleByThreeAndFive => (i) => _byThreeAndFive;
        public Func<int, string> Indivisible => (i) => i.ToString();
    }
}