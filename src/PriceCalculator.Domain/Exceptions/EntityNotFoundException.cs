using System.Runtime.Serialization;

namespace PriceCalculator.Domain.Exceptions;

public sealed class EntityNotFoundException : InfrastructureException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}