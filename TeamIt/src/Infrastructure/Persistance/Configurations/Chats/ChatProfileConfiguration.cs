using Domain.Entities.ProjectManager;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Chats;

namespace Infrastructure.Persistance.Configurations.Chats
{
    public class ChatProfileConfiguration : IEntityTypeConfiguration<ChatProfile>
    {
        public void Configure(EntityTypeBuilder<ChatProfile> builder)
        {
            builder.HasMany(cp => cp.Messages)
                .WithOne(m => m.SenderProfile)
                .HasForeignKey(m => m.SenderProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
