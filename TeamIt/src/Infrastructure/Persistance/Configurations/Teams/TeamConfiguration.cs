using Domain.Entities;
using Domain.Enums;
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
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(50);

            builder.HasOne(t => t.CreatorUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(t => t.Roles)
                .WithMany(r => r.Teams)
                .UsingEntity<Dictionary<string, object>>("RoleTeam",
                    t => t.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    r => r.HasOne<Team>().WithMany().HasForeignKey("TeamId"),
                    je => je.HasKey("RoleId", "TeamId"));
        }
    }
}