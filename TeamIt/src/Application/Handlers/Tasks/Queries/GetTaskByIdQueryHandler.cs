using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Tasks.Dto;
using Models.Tasks.Queries;

namespace Application.Handlers.Tasks.Queries
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        private Domain.Entities.ProjectManager.Task? _task;

        public GetTaskByIdQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var taskDto = _mapper.Map<TaskDto>(_task);
            return taskDto;
        }

        private async Task ValidateRequest(GetTaskByIdQuery request)
        {
            var project = await _context.Project.FindAsync(request.ProjectId);
            if (project is null)
                throw new ValidationException("Project with provided id does not exist");

            _task = project.GetTask(request.TaskId);
            if (_task == default)
                throw new ValidationException("Task with provided id does not exist in the project");
        }
    }
}