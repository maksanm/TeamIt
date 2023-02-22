using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Teams;

namespace Infrastructure.Persistance.Configurations.Teams
{
    public class TeamProfileConfiguration : IEntityTypeConfiguration<TeamProfile>
    {
        public void Configure(EntityTypeBuilder<TeamProfile> builder)
        {
            builder.HasMany(tp => tp.ProjectProfiles)
                .WithOne(pp => pp.TeamProfile)
                .HasForeignKey(pp => pp.TeamProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(tp => tp.ChatProfiles)
                .WithOne(cp => cp.TeamProfile)
                .HasForeignKey(cp => cp.TeamProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}