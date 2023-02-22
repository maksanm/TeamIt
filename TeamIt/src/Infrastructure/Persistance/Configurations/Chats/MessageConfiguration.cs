using Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Configurations.Chats
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(m => m.Text)
                .HasMaxLength(100);

            builder.HasOne(m => m.SenderProfile)
                .WithMany()
                .HasForeignKey(m => m.SenderProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
