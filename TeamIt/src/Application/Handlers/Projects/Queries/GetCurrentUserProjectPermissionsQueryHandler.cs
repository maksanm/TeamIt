using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities.ProjectManager;
using MediatR;
using Models.Permissions.Dto;
using Models.Projects.Dto;
using Models.Projects.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Projects.Queries
{
    public class GetCurrentUserProjectPermissionsQueryHandler : IRequestHandler<GetCurrentUserProjectPermissionsQuery, IList<PermissionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserProjectPermissionsQueryHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<PermissionDto>> Handle(GetCurrentUserProjectPermissionsQuery request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var currentUserProjectProfile = await _identityService.GetCurrentUserProjectProfileAsync(request.ProjectId);
            var permissionDtos = _mapper.Map<IList<PermissionDto>>(currentUserProjectProfile.Role.Permissions);
            return permissionDtos;
        }

        private async System.Threading.Tasks.Task ValidateRequest(GetCurrentUserProjectPermissionsQuery request)
        {
            var project = await _context.Project.FindAsync(request.ProjectId);
            if (project is null)
                throw new ValidationException("Project with provided id does not exist");
        }
    }
}