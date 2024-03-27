using System.Runtime.Serialization;

namespace PriceCalculator.Bll.Exceptions;

public class GoodsNotFoundException : Exception
{
    public GoodsNotFoundException()
    {
    }

    protected GoodsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public GoodsNotFoundException(string? message) : base(message)
    {
    }

    public GoodsNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}