using Domain.Entities.ProjectManager;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Configurations.ProjectManager
{
    public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.ProjectManager.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProjectManager.Task> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.HasOne(t => t.AssigneeProfile)
                .WithMany(pp => pp.Tasks)
                .HasForeignKey(t => t.AssigneeProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(t => t.ParentTask)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(t => t.ParentTaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}