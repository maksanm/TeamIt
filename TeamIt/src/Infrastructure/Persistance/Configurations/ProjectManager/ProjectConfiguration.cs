using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.ProjectManager;

namespace Infrastructure.Persistance.Configurations.ProjectManager
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.HasOne(t => t.CreatorTeam)
                .WithMany()
                .HasForeignKey(t => t.CreatorTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Tasks)
               .WithOne(t => t.Project)
               .HasForeignKey(t => t.ProjectId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Profiles)
               .WithOne(pp => pp.Project)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}