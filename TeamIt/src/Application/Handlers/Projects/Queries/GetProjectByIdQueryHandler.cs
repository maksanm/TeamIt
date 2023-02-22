using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Projects.Dto;
using Models.Projects.Queries;

namespace Application.Handlers.Projects.Queries
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Project.FindAsync(request.ProjectId);
            if (project is null)
                throw new ValidationException();

            var projectDto = _mapper.Map<ProjectDto>(project);
            return projectDto;
        }
    }
}