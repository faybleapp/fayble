using System.Runtime.Serialization;

namespace Fayble.Core.Exceptions;

[Serializable]
public sealed class NotFoundException : Exception
{
    public NotFoundException() : base()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    private NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}