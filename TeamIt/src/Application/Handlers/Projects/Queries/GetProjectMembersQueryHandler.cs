using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities.ProjectManager;
using MediatR;
using Models.Projects.Dto;
using Models.Projects.Queries;

namespace Application.Handlers.Projects.Queries
{
    public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, ProjectMembersDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        private Project? _project;

        public GetProjectMembersQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectMembersDto> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var projectMembersDto = _mapper.Map<ProjectMembersDto>(_project);
            ClearDifferentTeamsSameMembers(projectMembersDto);
            return projectMembersDto;
        }

        private async System.Threading.Tasks.Task ValidateRequest(GetProjectMembersQuery request)
        {
            _project = await _context.Project.FindAsync(request.ProjectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private void ClearDifferentTeamsSameMembers(ProjectMembersDto projectMembersDto)
        {
            projectMembersDto.Members.ForEach(teamMember =>
            {
                var projectMembersFromTeamMemberProfiles = _project!.Profiles
                    .Select(projectProfile => projectProfile.TeamProfile)
                    .Where(teamProfile => teamProfile.Team.Id == teamMember.TeamInfo.Id);
                teamMember.Members
                    .RemoveAll(member => !projectMembersFromTeamMemberProfiles
                        .Any(teamProfile => member.User.Id == teamProfile.User.Id));
            });
        }
    }
}