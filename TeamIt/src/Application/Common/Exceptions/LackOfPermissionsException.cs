namespace Application.Common.Exceptions
{
    public class LackOfPermissionsException : CustomApplicationException
    {
        public LackOfPermissionsException(string message = "User does not have enough permission to perform this action")
           : base(message)
        { }
    }
}