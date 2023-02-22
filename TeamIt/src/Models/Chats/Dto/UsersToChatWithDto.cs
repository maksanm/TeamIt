using Models.Projects.Dto;
using Models.Teams.Dto;

namespace Models.Chats.Dto
{
    public class UsersToChatWithDto
    {
         public List<ProjectMembersDto> ProjectUsers { get; set; }
         public List<TeamMembersDto> TeamUsers { get; set; }

        public UsersToChatWithDto() 
        {
            ProjectUsers = new List<ProjectMembersDto>();
            TeamUsers = new List<TeamMembersDto>();
        }
    }
}
