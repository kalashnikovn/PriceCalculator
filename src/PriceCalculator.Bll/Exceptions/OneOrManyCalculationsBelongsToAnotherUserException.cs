using System.Runtime.Serialization;

namespace PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    public IReadOnlyList<long>? DifferentCalculationIds { get; }
    
    public OneOrManyCalculationsBelongsToAnotherUserException(string message, IReadOnlyList<long> differentCalculationIds)
        : base(message)
    {
        DifferentCalculationIds = differentCalculationIds;
    }
    
    public OneOrManyCalculationsBelongsToAnotherUserException()
    {
    }

    protected OneOrManyCalculationsBelongsToAnotherUserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public OneOrManyCalculationsBelongsToAnotherUserException(string? message) : base(message)
    {
    }

    public OneOrManyCalculationsBelongsToAnotherUserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}