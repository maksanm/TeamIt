using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Teams.Dto;
using Models.Teams.Queries;

namespace Application.Handlers.Teams.Queries
{
    public class GetCurrentUserJoinTeamRequestsQueryHandler : IRequestHandler<GetCurrentUserJoinTeamRequestsQuery, IList<JoinTeamRequestDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserJoinTeamRequestsQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<JoinTeamRequestDto>> Handle(GetCurrentUserJoinTeamRequestsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();

            var joinTeamRequestDtos = _mapper.Map<IList<JoinTeamRequestDto>>(currentUser.JoinTeamRequests);
            return joinTeamRequestDtos;
        }
    }
}
