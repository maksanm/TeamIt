namespace Application.Common.Exceptions
{
    public class UnauthorizedException : CustomApplicationException
    {
        public UnauthorizedException(string message = "User is unauthorized")
           : base(message)
        { }
    }
}