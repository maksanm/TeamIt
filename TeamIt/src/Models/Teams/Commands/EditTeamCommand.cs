using MediatR;
using Microsoft.AspNetCore.Http;
using Models.Teams.Dto;

namespace Models.Teams.Commands
{
    public class EditTeamCommand : IRequest
    {
        public long TeamId { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}