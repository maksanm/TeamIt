using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.Extensions.Hosting;
using Domain.Enums;

namespace Infrastructure.Persistance.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50);

            builder.HasData(
                new Role() { Id = (long)BasicRoleEnum.TEAM_CREATOR, Name = "Team creator" },
                new Role() { Id = (long)BasicRoleEnum.GUEST, Name = "Guest" }
            );

            builder.HasMany(r => r.Permissions)
                .WithMany(t => t.Roles)
                .UsingEntity<Dictionary<string, object>>("RolePermission",
                    p => p.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    je =>
                    {
                        je.HasKey("RoleId", "PermissionId");
                        //Team creator
                        je.HasData(
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_EDIT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_DELETE },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_ADD_USER },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_KICK_USER },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_ASSIGN_ROLE },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_MANAGE_ROLE },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_CREATE_PROJECT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_ADD_TO_PROJECT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_LEAVE_PROJECT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.TEAM_DELETE_PROJECT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_EDIT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_ADD_TEAM },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_KICK_TEAM },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_SET_LIMIT_ROLE },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_CREATE_TASK },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_ASSIGN_TASK },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_CREATE_SUBTASK },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_EDIT_TASK },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.PM_DELETE_TASK },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.CHAT_EDIT },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.CHAT_DELETE },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.CHAT_ADD_USER },
                            new { RoleId = (long)1, PermissionId = PermissionEnum.CHAT_SEND_IMAGE });
                        //Team guest doesn't have any team permissions
                        //je.HasData();
                    });
        }
    }
}