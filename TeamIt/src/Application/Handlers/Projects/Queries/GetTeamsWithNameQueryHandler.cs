using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Projects.Queries;
using Models.Teams.Dto;

namespace Application.Handlers.Projects.Queries
{
    public class GetTeamsToAddToProjectQueryHandler : IRequestHandler<GetTeamsToAddToProjectQuery, IList<TeamDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;


        public GetTeamsToAddToProjectQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<IList<TeamDto>> Handle(GetTeamsToAddToProjectQuery request, CancellationToken cancellationToken)
        {
            var currentUserProjectProfile = await _identityService.GetCurrentUserProjectProfileAsync(request.ProjectId);
            var teams = currentUserProjectProfile.User.TeamProfiles
                .Where(profile => profile.Id != currentUserProjectProfile.TeamProfile.Id)
                .Select(profile => profile.Team);
            var teamDtos = _mapper.Map<IList<TeamDto>>(teams);
            return teamDtos;
        }
    }
}