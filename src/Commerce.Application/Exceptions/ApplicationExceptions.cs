namespace Commerce.Application.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
    protected AppException(string message, Exception? inner) : base(message, inner) { }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) { }
}

public sealed class ValidationException : AppException
{
    public ValidationException(string message) : base(message) { }
}

public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }
}

public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message) { }
}

public class ExternalServiceException : AppException
{
    public ExternalServiceException(string message, Exception? inner = null) : base(message, inner) { }
}

public sealed class ServiceBusException : ExternalServiceException
{
    public ServiceBusException(string message, Exception? inner = null) : base(message, inner) {}
}
public sealed class BlobStorageException : ExternalServiceException
{
    public BlobStorageException(string message, Exception? inner = null) : base(message, inner) {}
}