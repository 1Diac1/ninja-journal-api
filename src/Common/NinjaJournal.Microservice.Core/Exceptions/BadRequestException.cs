namespace NinjaJournal.Microservice.Core.Exceptions;

public class BadRequestException : Exception
{
    public IEnumerable<string> Errors { get; set; }

    public BadRequestException()
        : base()
    { }

    public BadRequestException(string message)
        : base(message)
    {
        Errors = new[] { message };
    }

    public BadRequestException(string message, Type type)
    {
        Errors = new[] { message + "(" + typeof(Type) + ")" };
    }

    public BadRequestException(string message, string property)
    {
        Errors = new[] { message + $"({property})" };
    }
    
    public BadRequestException(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    { }
}