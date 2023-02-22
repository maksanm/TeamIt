using Domain.Entities;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Enums;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Application.IntegrationTests
{
    public abstract class TestsBase
    {
        private WebApplicationFactory<Program> _factory;

        protected HttpClient _client;
        protected HttpClient _unauthorizedClient;
        protected string _currentUserId;

        [OneTimeSetUp]
        public async System.Threading.Tasks.Task OneTimeSetUp()
        {
            _factory = new WebApplicationFactory<Program>();

            var context = GetDbContext();
            context!.Database.EnsureDeleted();
            context!.Database.Migrate();

            _client = _factory.CreateClient();
            _unauthorizedClient = _factory.CreateClient();

            (var currentUser, var token) = await RegisterUser("UserName");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _currentUserId = currentUser.Id;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        protected ApplicationDbContext GetDbContext()
        {
            var serviceScope = _factory.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return context;
        }

        protected async System.Threading.Tasks.Task<(User, string)> RegisterUser(string userName)
        {
            var registerCommandDict = new Dictionary<string, string>
            {
                { "Login", userName },
                { "Password", "Password!123" },
                { "Name", "Name" },
                { "Surname", "Surname" }
            };
            var registerCommand = new HttpRequestMessage(HttpMethod.Post, "/users/register") 
            { 
                Content = new FormUrlEncodedContent(registerCommandDict) 
            };
            var response = await _client.SendAsync(registerCommand);
            var token = await response.Content.ReadAsStringAsync();
            var context = GetDbContext();
            var user = context.User.FirstOrDefault(u => u.UserName == userName);
            return (user, token);
        }

        protected Team CreateTeam(string creatorIdIfNotCurrentUser = null, string name = "Name", int otherMembersCount = 0)
        {
            var team = new Team()
            {
                Name = name,
                CreatorUserId = creatorIdIfNotCurrentUser ?? _currentUserId,
                Profiles = new List<TeamProfile>(),
                Roles = new List<Role>()
            };
            team.Profiles.Add(new TeamProfile()
            {
                UserId = creatorIdIfNotCurrentUser ?? _currentUserId,
                Team = team,
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR
            });

            for (int i = 0; i < otherMembersCount; i++)
            {
                var otherMember = RegisterUser("OtherMember" + i.ToString()).Result.Item1;
                var otherMemberProfile = new TeamProfile()
                {
                    UserId = otherMember.Id,
                    Team = team,
                    RoleId = (long)BasicRoleEnum.GUEST
                };
                team.Profiles.Add(otherMemberProfile);
            }
            return team;
        }

        protected Project CreateProject(Team creatorTeam, string name = null, int tasksCount = 0)
        {
            var project = new Project()
            {
                Name = name ?? "Project",
                CreatorTeamId = creatorTeam.Id,
                Profiles = new List<ProjectProfile>(),
                Tasks = new List<Domain.Entities.ProjectManager.Task>(),
                UseOwnHierarchy = true,
                LimitRoleId = (long)BasicRoleEnum.TEAM_CREATOR //for easier testing
            };
            foreach (var teamProfile in creatorTeam.Profiles)
                project.Profiles.Add(new ProjectProfile()
                {
                    TeamProfileId = teamProfile.Id,
                    Tasks = new List<Domain.Entities.ProjectManager.Task>()
                });
            for (int i = 0; i < tasksCount; i++)
                project.Tasks.Add(CreateTask(project, "Task" + i.ToString()));
            return project;
        }
        
        protected Chat CreateChat(string creatorId, string userId, 
            Team baseTeam, Project baseProject, 
            string name = null, int messagesCount = 0)
        {
            var chat = new Chat()
            {
                Name = name ?? "Chat",
                BaseTeam = baseTeam,
                BaseProject = baseProject,
                Profiles = new List<ChatProfile>(),
                Messages = new List<Message>(),
            };
            var creatorProfile = CreateChatProfile(creatorId, chat);
            var userProfile = CreateChatProfile(userId, chat);
            chat.Profiles.Add(creatorProfile);
            chat.Profiles.Add(userProfile);
            for (int i = 0; i < messagesCount; i++)
                chat.Messages.Add(CreateMessage(i % 2 == 0 ? creatorProfile : userProfile, chat));
            return chat;
        }

        protected Domain.Entities.ProjectManager.Task CreateTask(Project project, string name = null)
        {
            var task = new Domain.Entities.ProjectManager.Task()
            {
                Name = name ?? "Task",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(1),
                State = TaskStateEnum.BACKLOG,
                Subtasks = new List<Domain.Entities.ProjectManager.Task>(),
                Project = project
            };
            return task;
        }

        protected ChatProfile CreateChatProfile(string userId, Chat chat) => 
            new ChatProfile()
            {
                Chat = chat,
                TeamProfile = chat.BaseTeam?.Profiles.FirstOrDefault(profile => profile.UserId == userId),
                ProjectProfile = chat.BaseProject?.Profiles.FirstOrDefault(profile => profile.User.Id == userId),
            };

        protected Message CreateMessage(ChatProfile senderProfile, Chat chat) =>
            new Message()
            {
                Chat = chat,
                SenderProfile = senderProfile,
                Text = "Sample text",
                Date = DateTime.Now.AddDays(-1),
            };
    }
}