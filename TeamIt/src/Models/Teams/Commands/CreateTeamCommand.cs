using MediatR;
using Microsoft.AspNetCore.Http;
using Models.Teams.Dto;

namespace Models.Teams.Commands
{
    public class CreateTeamCommand : IRequest
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}