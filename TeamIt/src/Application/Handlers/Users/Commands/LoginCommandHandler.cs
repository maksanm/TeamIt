using Application.Common.Interfaces;
using MediatR;
using Models.Users.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Users.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.LoginUserWithUsernameAsync(request);
            return token;
        }
    }
}