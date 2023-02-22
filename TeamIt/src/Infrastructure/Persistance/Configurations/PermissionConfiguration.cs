using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistance.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(p => p.Id)
                .HasConversion(new EnumToNumberConverter<PermissionEnum, long>());
            builder.Property(p => p.Name)
                .HasMaxLength(100);

            builder.HasData(
                new Permission() { Id = PermissionEnum.TEAM_EDIT, Name = "Edit team preferences" },
                new Permission() { Id = PermissionEnum.TEAM_DELETE, Name = "Delete team" },
                new Permission() { Id = PermissionEnum.TEAM_ADD_USER, Name = "Add new user to the team" },
                new Permission() { Id = PermissionEnum.TEAM_KICK_USER, Name = "Kick user from the team" },
                new Permission() { Id = PermissionEnum.TEAM_ASSIGN_ROLE, Name = "Assign role to user" },
                new Permission() { Id = PermissionEnum.TEAM_MANAGE_ROLE, Name = "Create, delete and set up role" },
                new Permission() { Id = PermissionEnum.TEAM_CREATE_PROJECT, Name = "Create project for team" },
                new Permission() { Id = PermissionEnum.TEAM_ADD_TO_PROJECT, Name = "Add team to other teams projects" },
                new Permission() { Id = PermissionEnum.TEAM_LEAVE_PROJECT, Name = "Leave other team project with the whole team" },
                new Permission() { Id = PermissionEnum.TEAM_DELETE_PROJECT, Name = "Delete project created by team" },
                new Permission() { Id = PermissionEnum.PM_EDIT, Name = "Edit project preferences" },
                new Permission() { Id = PermissionEnum.PM_ADD_TEAM, Name = "Add new teams to the project" },
                new Permission() { Id = PermissionEnum.PM_KICK_TEAM, Name = "Kick team from the project" },
                new Permission() { Id = PermissionEnum.PM_SET_LIMIT_ROLE, Name = "Set role that restricts third party teams permissions" },
                new Permission() { Id = PermissionEnum.PM_CREATE_TASK, Name = "Create new tasks" },
                new Permission() { Id = PermissionEnum.PM_ASSIGN_TASK, Name = "Assign user to complete the task" },
                new Permission() { Id = PermissionEnum.PM_CREATE_SUBTASK, Name = "Create subtasks for existing tasks" },
                new Permission() { Id = PermissionEnum.PM_EDIT_TASK, Name = "Change task state, description, start date and deadline" },
                new Permission() { Id = PermissionEnum.PM_DELETE_TASK, Name = "Delete task with all subtasks" },
                new Permission() { Id = PermissionEnum.CHAT_EDIT, Name = "Change chat name and picture" },
                new Permission() { Id = PermissionEnum.CHAT_DELETE, Name = "Delete chat" },
                new Permission() { Id = PermissionEnum.CHAT_ADD_USER, Name = "Add user from base team or project to the chat" },
                new Permission() { Id = PermissionEnum.CHAT_SEND_IMAGE, Name = "Send image to the chat" }
            );
        }
    }
}