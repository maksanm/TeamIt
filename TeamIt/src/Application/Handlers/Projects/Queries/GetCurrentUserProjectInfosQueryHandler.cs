using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Projects.Dto;
using Models.Projects.Queries;

namespace Application.Handlers.Projects.Queries
{
    public class GetCurrentUserProjectInfosQueryHandler : IRequestHandler<GetCurrentUserProjectInfosQuery, IList<ProjectInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserProjectInfosQueryHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<ProjectInfoDto>> Handle(GetCurrentUserProjectInfosQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();

            var currentUserProjects = _context.ProjectProfile
                .Where(pp => pp.TeamProfile.User.Id == currentUser.Id)
                .Select(pp => pp.Project)
                .ToList();
            var projectInfoDtos = _mapper.Map<IList<ProjectInfoDto>>(currentUserProjects);
            return projectInfoDtos;
        }
    }
}