using System.Runtime.Serialization;

namespace PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsNotFoundException : Exception
{
    public OneOrManyCalculationsNotFoundException()
    {
    }

    protected OneOrManyCalculationsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public OneOrManyCalculationsNotFoundException(string? message) : base(message)
    {
    }

    public OneOrManyCalculationsNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}