using Application.Common.Interfaces;
using MediatR;
using Models.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Users.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.CreateUserAsync(request);
            return token;
        }
    }
}