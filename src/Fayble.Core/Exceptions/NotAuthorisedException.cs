using System.Runtime.Serialization;

namespace Fayble.Core.Exceptions;

[Serializable]
public sealed class NotAuthorisedException : Exception
{
    public NotAuthorisedException() : base()
    {
    }

    public NotAuthorisedException(string message) : base(message)
    {
    }

    public NotAuthorisedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    private NotAuthorisedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}