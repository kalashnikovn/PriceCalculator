using System.Runtime.Serialization;

namespace PriceCalculator.Domain.Exceptions;

public sealed class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DomainException(string? message) : base(message)
    {
    }

    public DomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}