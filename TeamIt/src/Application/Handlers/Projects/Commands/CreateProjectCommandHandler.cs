using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities.Teams;
using Domain.Entities;
using MediatR;
using Models.Projects.Commands;
using Domain.Entities.ProjectManager;
using Application.Common.Exceptions;
using Domain.Enums;

namespace Application.Handlers.Projects.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IPermissionsProvider _permissionsProvider;
        private readonly IBasicRolesProvider _basicRolesProvider;
        private readonly IImageService _imageService;

        private Team? _creatorTeam;

        public CreateProjectCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IPermissionsProvider permissionsProvider,
            IBasicRolesProvider basicRolesProvider,
            IImageService imageService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _permissionsProvider = permissionsProvider;
            _basicRolesProvider = basicRolesProvider;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.CreatorTeamId, PermissionEnum.TEAM_CREATE_PROJECT);

            var newProject = CreateNewProject(request);
            _context.Project.Add(newProject);
            await _context.SaveChangesAsync(cancellationToken);
            if (request.Image is not null)
                await _imageService.SetProjectPicture(newProject.Id, request.Image);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(CreateProjectCommand request)
        {
            ValidateProjectName(request.Name);
            await ValidateCreatorTeam(request.CreatorTeamId);
            ValidateLimitRole(request);
            if (request.DeadLine is not null)
                ValidateDeadLine(request);
        }

        private Project CreateNewProject(CreateProjectCommand request)
        {
            var project = new Project()
            {
                CreatorTeam = _creatorTeam!,
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                DeadLine = request.DeadLine,
                LimitRole = CreateLimitRole(request),
                UseOwnHierarchy = request.UseOwnHierarchy,
                Tasks = new List<Domain.Entities.ProjectManager.Task>(),
            };
            project.Profiles = CreateProjectProfiles(project);
            return project;
        }

        private static void ValidateProjectName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Project name must be provided");
        }

        private async System.Threading.Tasks.Task ValidateCreatorTeam(long creatorTeamId)
        {
            _creatorTeam = await _context.Team.FindAsync(creatorTeamId);
            if (_creatorTeam is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private static void ValidateLimitRole(CreateProjectCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.LimitRoleName) && request.LimitRolePermissionIds!.Any())
                throw new ValidationException("Limit role name must be provided");
        }

        private void ValidateDeadLine(CreateProjectCommand request)
        {
            if (request.StartDate is not null && request.DeadLine < request.StartDate)
                throw new ValidationException("Deadline date should be greater than start date");
            if (request.DeadLine < DateTime.Now)
                throw new ValidationException("Invalid deadline date");
        }

        private Role CreateLimitRole(CreateProjectCommand request) =>
            !string.IsNullOrWhiteSpace(request.LimitRoleName)
            ? new Role()
            {
                Name = request.LimitRoleName,
                Permissions = _permissionsProvider
                    .PermissionsWithIds(request.LimitRolePermissionIds ?? new List<int>())
                    .ToList(),
            }
            : _basicRolesProvider.GuestRole();

        private List<ProjectProfile> CreateProjectProfiles(Project project) =>
            _creatorTeam!
                .Profiles
                .Select(teamProfile =>
                    new ProjectProfile()
                    {
                        TeamProfile = teamProfile,
                        Project = project,
                        Tasks = new List<Domain.Entities.ProjectManager.Task>()
                    })
                .ToList();
    }
}