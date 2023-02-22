namespace Application.Common.Exceptions
{
    public class ValidationException : CustomApplicationException
    {
        public ValidationException(string message = "Invalid data provided")
           : base(message)
        { }
    }
}