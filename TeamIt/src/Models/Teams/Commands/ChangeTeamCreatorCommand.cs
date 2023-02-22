using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Teams.Commands
{
    public class ChangeTeamCreatorCommand : IRequest
    {
        public long TeamId { get; set; }
        public string NewTeamCreatorUserId { get; set; }
    }
}