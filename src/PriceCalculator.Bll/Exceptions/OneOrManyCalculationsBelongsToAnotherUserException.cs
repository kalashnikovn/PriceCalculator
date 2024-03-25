using System.Runtime.Serialization;

namespace PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    public long[] WrongCalculationIds { get; }

    public OneOrManyCalculationsBelongsToAnotherUserException(string message, long[] wrongCalculationIds)
        : base(message)
    {
        WrongCalculationIds = wrongCalculationIds;
    }
}