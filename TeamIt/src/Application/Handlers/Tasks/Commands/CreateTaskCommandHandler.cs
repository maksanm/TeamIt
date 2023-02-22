using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Tasks.Commands;

namespace Application.Handlers.Tasks.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IIdentityService _identityService;

        private Project? _project;
        private Domain.Entities.ProjectManager.Task? _parentTask;
        private ProjectProfile? _assigneeProfile;

        public CreateTaskCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IIdentityService identityService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            var newTask = CreateNewTask(request);

            if (_parentTask is not null)
            {
                await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_CREATE_SUBTASK);
                _parentTask.Subtasks.Add(newTask);
            }
            else
            {
                await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_CREATE_TASK);
                _project!.Tasks.Add(newTask);
            }
            if (_assigneeProfile is not null)
                _assigneeProfile.Tasks.Add(newTask);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(CreateTaskCommand request)
        {
            await ValidateProject(request.ProjectId);
            if (request.ParentTaskId > 0)
                ValidateParentTask((long)request.ParentTaskId);
            if (!string.IsNullOrEmpty(request.AssignedUserId))
                ValidateAssignedUser(request.AssignedUserId);
            ValidateTaskName(request.Name);
            if (request.DeadLine is not null)
                ValidateDeadline(request);
        }

        private Domain.Entities.ProjectManager.Task CreateNewTask(CreateTaskCommand request) =>
           new Domain.Entities.ProjectManager.Task()
           {
               Name = request.Name,
               Description = request.Description,
               StartDate= request.StartDate,
               DeadLine = request.DeadLine,
               State = TaskStateEnum.BACKLOG,
               ParentTask = _parentTask!,
               AssigneeProfile = _assigneeProfile!,
           };

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private void ValidateParentTask(long taskId)
        {
            _parentTask = _project!.Tasks.FirstOrDefault(task => task.Id == taskId);
            if (_parentTask == default)
                throw new ValidationException("Parent task with provided id does not exist in the project");
            if (_parentTask.ParentTask is not null)
                throw new ValidationException("Only one level subtasks are allowed");
        }

        private void ValidateAssignedUser(string userId)
        {
            _assigneeProfile = _project!.Profiles.FirstOrDefault(pp => pp.User.Id == userId);
            if (_assigneeProfile == default)
                throw new ValidationException("User with provided id does is not a member of the project");
        }

        private void ValidateTaskName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Task name must be provided");
        }

        private void ValidateDeadline(CreateTaskCommand request)
        {
            if (request.StartDate is not null && request.DeadLine < request.StartDate)
                throw new ValidationException("Deadline date should be greater than start date");
            if (request.DeadLine < DateTime.Now)
                throw new ValidationException("Invalid deadline date");
        }
    }
}