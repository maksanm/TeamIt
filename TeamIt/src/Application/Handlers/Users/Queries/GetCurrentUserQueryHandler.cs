using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Users.Commands;
using Models.Users.Dto;
using Models.Users.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Users.Queries
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetCurrentUserAsync();
            if (user is null)
                throw new UnauthorizedException();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}