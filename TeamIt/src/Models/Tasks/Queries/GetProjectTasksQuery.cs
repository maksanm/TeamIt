using MediatR;
using Models.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tasks.Queries
{
    public class GetProjectTasksQuery : IRequest<IList<TaskDto>>
    {
        public long ProjectId { get; set; }
    }
}