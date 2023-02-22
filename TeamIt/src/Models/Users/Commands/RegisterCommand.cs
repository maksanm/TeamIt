using MediatR;
using Microsoft.AspNetCore.Http;

namespace Models.Users.Commands
{
    public class RegisterCommand : IRequest<string>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile? Image { get; set; }
    }
}