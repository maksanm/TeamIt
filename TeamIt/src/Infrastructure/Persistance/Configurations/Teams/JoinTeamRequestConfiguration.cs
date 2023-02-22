using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Teams;

namespace Infrastructure.Persistance.Configurations.Teams
{
    public class JoinTeamRequestConfiguration : IEntityTypeConfiguration<JoinTeamRequest>
    {
        public void Configure(EntityTypeBuilder<JoinTeamRequest> builder)
        {
            builder.HasOne(r => r.UserToAdd)
                .WithMany(u => u.JoinTeamRequests)
                .HasForeignKey(r => r.UserToAddId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.UserToAdd)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Team)
                .WithMany()
                .HasForeignKey(r => r.TeamId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
