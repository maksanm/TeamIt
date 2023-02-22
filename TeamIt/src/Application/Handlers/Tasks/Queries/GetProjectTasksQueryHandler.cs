using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Tasks.Dto;
using Models.Tasks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Tasks.Queries
{
    public class GetProjectTasksQueryHandler : IRequestHandler<GetProjectTasksQuery, IList<TaskDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectTasksQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<TaskDto>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Project.FindAsync(request.ProjectId);
            if (project is null)
                throw new ValidationException("Project with provided id does not exist");

            var taskDtos = _mapper.Map<IList<TaskDto>>(project.Tasks);
            return taskDtos;
        }
    }
}