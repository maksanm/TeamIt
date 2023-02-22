using MediatR;
using Models.Permissions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Projects.Queries
{
    public class GetCurrentUserProjectPermissionsQuery : IRequest<IList<PermissionDto>>
    {
        public long ProjectId { get; set; }
    }
}