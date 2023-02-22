using Application.Common.Interfaces;
using Application.Common.Providers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Permissions.Dto;
using Models.Permissions.Queries;

namespace Application.Handlers.Permissions.Queries
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IList<PermissionDto>>
    {
        private readonly IPermissionsProvider _provider;
        private readonly IMapper _mapper;

        public GetPermissionsQueryHandler(IPermissionsProvider provider, IMapper mapper)
        {
            _provider = provider;
            _mapper = mapper;
        }

        public async Task<IList<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = _provider.AllPermissions();
            var permissionDtos = _mapper.Map<List<PermissionDto>>(permissions);
            return permissionDtos;
        }
    }
}