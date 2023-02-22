using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired(false);
            builder.Property(u => u.Surname)
                .HasMaxLength(50)
                .IsRequired(false);
        }
    }
}