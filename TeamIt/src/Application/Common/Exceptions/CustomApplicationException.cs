namespace Application.Common.Exceptions
{
    // Used in for error handling in controllers
    public class CustomApplicationException : Exception
    {
        public CustomApplicationException(string message) : base(message)
        { }
    }
}
