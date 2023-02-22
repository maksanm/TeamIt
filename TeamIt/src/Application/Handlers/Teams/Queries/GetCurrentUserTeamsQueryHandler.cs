using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Teams.Dto;
using Models.Teams.Queries;

namespace Application.Handlers.Teams.Queries
{
    public class GetCurrentUserTeamsQueryHandler : IRequestHandler<GetCurrentUserTeamsQuery, IList<TeamDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserTeamsQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<TeamDto>> Handle(GetCurrentUserTeamsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();

            var currentUserTeams = currentUser.TeamProfiles
                .Select(tp => tp.Team);
            var teamDtos = _mapper.Map<IList<TeamDto>>(currentUserTeams);
            return teamDtos;
        }
    }
}