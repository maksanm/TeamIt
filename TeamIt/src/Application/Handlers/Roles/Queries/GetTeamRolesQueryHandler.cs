using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Roles.Dto;
using Models.Roles.Queries;
using Models.Teams.Dto;
using Models.Teams.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Roles.Queries
{
    public class GetTeamRolesQueryHandler : IRequestHandler<GetTeamRolesQuery, IList<RoleDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTeamRolesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<RoleDto>> Handle(GetTeamRolesQuery request, CancellationToken cancellationToken)
        {
            var team = await _context.Team.FindAsync(request.TeamId);
            if (team is null)
                throw new ValidationException();
            var roleDtos = _mapper.Map<IList<RoleDto>>(team.Roles);
            return roleDtos;
        }
    }
}