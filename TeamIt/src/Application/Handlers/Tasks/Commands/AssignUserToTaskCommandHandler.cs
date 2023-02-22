using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Tasks.Commands;

namespace Application.Handlers.Tasks.Commands
{
    public class AssignUserToTaskCommandHandler : IRequestHandler<AssignUserToTaskCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Project? _project;
        private Domain.Entities.ProjectManager.Task? _task;
        private ProjectProfile? _assigneeProfile;

        public AssignUserToTaskCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(AssignUserToTaskCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_ASSIGN_TASK);

            _task!.AssigneeProfile = _assigneeProfile!;
            _assigneeProfile!.Tasks.Add(_task);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(AssignUserToTaskCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateTask(request.TaskId);
            ValidateAssignedUser(request.UserId);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private void ValidateTask(long taskId)
        {
            _task = _project!.GetTask(taskId);
            if (_task == default)
                throw new ValidationException("Task with provided id does not exist in the project");
        }

        private void ValidateAssignedUser(string userId)
        {
            _assigneeProfile = _project!.Profiles.FirstOrDefault(pp => pp.User.Id == userId);
            if (_assigneeProfile == default && !string.IsNullOrEmpty(userId))
                throw new ValidationException("User with provided id is not a member of the project");
        }
    }
}