using Domain.Entities.Chats;
using Domain.Entities.Teams;

namespace Domain.Entities.ProjectManager
{
    public class ProjectProfile : IProfile
    {
        public long Id { get; set; }
        public long TeamProfileId { get; set; }
        public virtual TeamProfile TeamProfile { get; set; }
        public long ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual IList<Task> Tasks { get; set; }
        public virtual IList<ChatProfile> ChatProfiles { get; set; }

        public Role Role => GetRole();
        public User User => TeamProfile.User;

        private Role GetRole()
        {
            var teamRole = TeamProfile.Role;
            var limitRole = Project.LimitRole;
            var fromCreatorTeam = Project.CreatorTeam.Profiles.Contains(TeamProfile);
            if (fromCreatorTeam)
                return teamRole;
            if (Project.UseOwnHierarchy && limitRole is not null)
                return CreateIntersectionRole(teamRole, limitRole);
            else if (Project.UseOwnHierarchy)
                return teamRole;
            else return limitRole!;
        }

        private static Role CreateIntersectionRole(Role first, Role second)
        {
            return new Role()
            {
                Name = $"{second.Name}|{first.Name}",
                Permissions = first.Permissions
                    .Where(firstPerm => second.Permissions
                        .Any(secondPerm => firstPerm.Id == secondPerm.Id))
                    .ToList()
            };
        }
    }
}