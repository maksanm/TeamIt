using MediatR;

namespace Models.Users.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}