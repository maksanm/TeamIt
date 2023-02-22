using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationDbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<TeamProfile> TeamProfile { get; set; }
        public DbSet<JoinTeamRequest> JoinTeamRequest { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Domain.Entities.ProjectManager.Task> Task { get; set; }
        public DbSet<ProjectProfile> ProjectProfile { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<ChatProfile> ChatProfile { get; set; }
        public DbSet<Message> Message { get; set; }

        public ApplicationDbContext(
            DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        //user interactions tracking here
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) =>
            base.SaveChangesAsync(cancellationToken);

        public override int SaveChanges()
        {
            base.Database.OpenConnection();
            int result;
            try
            {
                base.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Role ON");
                result = base.SaveChanges();
                base.Database.ExecuteSql($"SET IDENTITY_INSERT dbo.Role OFF");
            }
            finally
            {
                base.Database.CloseConnection();
            }
            return result;
        }
    }
}