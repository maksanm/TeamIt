using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Teams.Dto;
using Models.Users.Commands;

namespace Application.Handlers.Teams.Queries
{
    public class GetTeamMembersQueryHandler : IRequestHandler<GetTeamMembersQuery, IList<TeamMemberDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTeamMembersQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<TeamMemberDto>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var team = await _context.Team.FindAsync(request.TeamId);
            if (team is null)
                throw new ValidationException("Team with provided id does not exist");

            var teamMemberDtos = _mapper.Map<IList<TeamMemberDto>>(team.Profiles);
            return teamMemberDtos;
        }
    }
}