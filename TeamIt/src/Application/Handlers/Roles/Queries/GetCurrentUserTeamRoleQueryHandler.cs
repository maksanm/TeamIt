using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Roles.Dto;
using Models.Roles.Queries;

namespace Application.Handlers.Roles.Queries
{
    public class GetCurrentUserTeamRoleQueryHandler : IRequestHandler<GetCurrentUserTeamRoleQuery, RoleDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserTeamRoleQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<RoleDto> Handle(GetCurrentUserTeamRoleQuery request, CancellationToken cancellationToken)
        {
            var profile = await _identityService.GetCurrentUserTeamProfileAsync(request.TeamId);
            var roleDto = _mapper.Map<RoleDto>(profile.Role);
            return roleDto;
        }
    }
}