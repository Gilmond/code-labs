using System;

namespace Ci.Vso.Lib
{
    public interface IFizzBuzzDictionary
    {
        Func<int, string> DivisibleByThree { get; }
        Func<int, string> DivisibleByFive { get; }
        Func<int, string> DivisibleByThreeAndFive { get; }
        Func<int, string> Indivisible { get; }
    }
}