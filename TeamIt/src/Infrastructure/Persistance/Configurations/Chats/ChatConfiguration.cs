using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Chats;

namespace Infrastructure.Persistance.Configurations.Chats
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(c => c.Name)
                .HasMaxLength(50);

            builder.HasOne(c => c.BaseTeam)
                .WithMany()
                .HasForeignKey(c => c.BaseTeamId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.BaseProject)
                .WithMany()
                .HasForeignKey(c => c.BaseProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Messages)
               .WithOne(m => m.Chat)
               .HasForeignKey(m => m.ChatId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Profiles)
               .WithOne(p => p.Chat)
               .HasForeignKey(p => p.ChatId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
