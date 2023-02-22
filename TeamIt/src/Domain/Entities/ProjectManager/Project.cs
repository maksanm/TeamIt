using Domain.Entities.Teams;

namespace Domain.Entities.ProjectManager
{
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool UseOwnHierarchy { get; set; }
        public long? CreatorTeamId { get; set; }
        public virtual Team CreatorTeam { get; set; }
        public long LimitRoleId { get; set; }
        public virtual Role LimitRole { get; set; }
        public long? PictureId { get; set; }
        public virtual Picture? Picture { get; set; }
        public virtual IList<Task> Tasks { get; set; }
        public virtual IList<ProjectProfile> Profiles { get; set; }

        public Task? GetTask(long taskId) => 
            Tasks.SelectMany(task => task.Subtasks)
                 .Concat(Tasks)
                 .FirstOrDefault(task => task.Id == taskId);

        public List<Team> GetProjectTeamsList() => 
            Profiles.Select(profile => profile.TeamProfile.Team)
                    .Distinct()
                    .ToList();
    }
}