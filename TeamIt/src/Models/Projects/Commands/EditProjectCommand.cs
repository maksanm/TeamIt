using MediatR;
using Microsoft.AspNetCore.Http;

namespace Models.Projects.Commands
{
    public class EditProjectCommand : IRequest
    {
        public long ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public IFormFile? Image { get; set; }
    }
}