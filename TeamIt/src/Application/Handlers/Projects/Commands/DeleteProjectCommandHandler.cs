using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Chats.Commands;
using Models.Projects.Commands;

namespace Application.Handlers.Projects.Commands
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        private Project? _project;

        public DeleteProjectCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IIdentityService identityService,
            IMediator mediator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            if (request.ValidatePermissions)
                await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.TEAM_DELETE_PROJECT);

            await ClearProjectRelatedEntities(_project!);
            _context.Project.Remove(_project!);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(DeleteProjectCommand request)
        {
            await ValidateProject(request.ProjectId);
            await ValidateProjectTeamCreator(request.ProjectId);
        }

        private async System.Threading.Tasks.Task ClearProjectRelatedEntities(Project project)
        {
            var chatsCreatedByProject = GetChatsCreatedByProject();
            for (var i = 0; i < chatsCreatedByProject.Count(); i++)
                await _mediator.Send(new DeleteChatCommand() { ChatId = chatsCreatedByProject.ElementAt(i).Id, ValidatePermissions = false });
            project.Tasks
                .ToList()
                .ForEach(task =>
                {
                    _context.Task.RemoveRange(task.Subtasks);
                    _context.Task.Remove(task);
                });
            _context.ProjectProfile.RemoveRange(project.Profiles);

            if (!Enum.IsDefined(typeof(BasicRoleEnum), (int)project.LimitRoleId))
                _context.Role.Remove(project.LimitRole);
        }

        private List<Chat> GetChatsCreatedByProject() =>
            _project!.Profiles
                .SelectMany(pp => pp.ChatProfiles)
                .Select(cp => cp.Chat)
                .Where(chat => chat.BaseProject?.Id == _project.Id)
                .Distinct()
                .ToList();

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private async System.Threading.Tasks.Task ValidateProjectTeamCreator(long projectId)
        {
            var curentUserProjectProfile = await _identityService.GetCurrentUserProjectProfileAsync(projectId);
            if (curentUserProjectProfile.TeamProfile.TeamId != _project!.CreatorTeamId)
                throw new LackOfPermissionsException("Only team creator members with corresponding permissions can delete the task");
        }
    }
}