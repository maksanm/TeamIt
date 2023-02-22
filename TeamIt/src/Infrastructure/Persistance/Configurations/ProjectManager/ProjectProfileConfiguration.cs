using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Configurations.ProjectManager
{
    public class ProjectProfileConfiguration : IEntityTypeConfiguration<ProjectProfile>
    {
        public void Configure(EntityTypeBuilder<ProjectProfile> builder)
        {
            builder.HasMany(pp => pp.ChatProfiles)
                .WithOne(cp => cp.ProjectProfile)
                .HasForeignKey(cp => cp.ProjectProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
