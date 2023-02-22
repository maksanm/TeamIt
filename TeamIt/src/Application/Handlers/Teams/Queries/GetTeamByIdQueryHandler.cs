using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Teams.Dto;
using Models.Teams.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Teams.Queries
{
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTeamByIdQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _context.Team.FindAsync(request.TeamId);
            if (team is null)
                throw new ValidationException("Team with provided id does not exist");

            var teamDto = _mapper.Map<TeamDto>(team);
            return teamDto;
        }
    }
}