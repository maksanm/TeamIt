using Domain.Entities;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> User { get; set; }
        DbSet<Picture> Picture { get; set; }
        DbSet<Permission> Permission { get; set; }
        DbSet<Team> Team { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<TeamProfile> TeamProfile { get; set; }
        DbSet<JoinTeamRequest> JoinTeamRequest { get; set; }
        DbSet<Project> Project { get; set; }
        DbSet<Domain.Entities.ProjectManager.Task> Task { get; set; }
        DbSet<ProjectProfile> ProjectProfile { get; set; }
        DbSet<Chat> Chat { get; set; }
        DbSet<ChatProfile> ChatProfile { get; set; }
        DbSet<Message> Message { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        int SaveChanges();
    }
}