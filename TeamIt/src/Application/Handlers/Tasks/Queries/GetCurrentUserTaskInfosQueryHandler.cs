using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Tasks.Dto;
using Models.Tasks.Queries;

namespace Application.Handlers.Tasks.Queries
{
    public class GetCurrentUserTaskInfosQueryHandler : IRequestHandler<GetCurrentUserTaskInfosQuery, IList<TaskInfoDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserTaskInfosQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<TaskInfoDto>> Handle(GetCurrentUserTaskInfosQuery request, CancellationToken cancellationToken)
        {
            var currentUserProjectProfile = await _identityService.GetCurrentUserProjectProfileAsync(request.ProjectId);
            var taskInfoDtos = _mapper.Map<IList<TaskInfoDto>>(currentUserProjectProfile.Tasks);
            return taskInfoDtos;
        }
    }
}