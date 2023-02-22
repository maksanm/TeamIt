using Domain.Enums;

namespace Domain.Entities.ProjectManager
{
    public class Task
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public TaskStateEnum State { get; set; }
        public long? AssigneeProfileId { get; set; }
        public virtual ProjectProfile AssigneeProfile { get; set; }
        public long? ParentTaskId { get; set; }
        public virtual Task ParentTask { get; set; }
        public virtual IList<Task> Subtasks { get; set; }
        public long? ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}