using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public long? ImageId { get; set; }
        public virtual Picture? Image { get; set; }
        public virtual IList<TeamProfile> TeamProfiles { get; set; }
        public virtual IList<JoinTeamRequest> JoinTeamRequests { get; set; }
    }
}