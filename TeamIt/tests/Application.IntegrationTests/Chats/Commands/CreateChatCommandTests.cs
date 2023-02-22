using System.Net;

namespace Application.IntegrationTests.Chats.Commands
{
    public class CreateChatCommandTests : TestsBase
    {
        private long _teamId;
        private long _projectId;
        private string _userId;

        [OneTimeSetUp]
        public void OneTimeSetUp_CreateProjectCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam(otherMembersCount: 2);
            _userId = team.Profiles
                .Select(p => p.UserId)
                .FirstOrDefault(id => id != _currentUserId);
            context.Team.Add(team);
            context.SaveChanges();
            var project = CreateProject(team);
            context.Project.Add(project);
            context.SaveChanges();
            _teamId = team.Id;
            _projectId = project.Id;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.ChatProfile.RemoveRange(context.ChatProfile);
            context.Chat.RemoveRange(context.Chat);
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldCreateProject_ForBaseTeamId()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "Name" },
                { "TeamId", $"{_teamId}" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            var context = GetDbContext();
            var chats = context.Chat.ToList();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats.Single().Profiles.Count, Is.EqualTo(2));
        }
        
        public async Task ShouldCreateProject_ForBaseProjectId()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "Name" },
                { "ProjectId", $"{_projectId}" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            var context = GetDbContext();
            var chats = context.Chat.ToList();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats.Single().Profiles.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForBothTeamAndProjectIdsProvided()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "Name" },
                { "TeamId", $"{_teamId}" },
                { "ProjectId", $"{_projectId}" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "" },
                { "TeamId", $"{_teamId}" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongUserId()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", "blablabla" },
                { "Name", "Name" },
                { "TeamId", $"{_teamId}" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "Name" },
                { "TeamId", "0" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "Name" },
                { "ProjectId", "0" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _client.SendAsync(createChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var createChatCommandDict = new Dictionary<string, string>
            {
                { "UserId", $"{_userId}" },
                { "Name", "Name" },
                { "TeamId", $"{_teamId}" },
            };
            var createChatCommand = new HttpRequestMessage(HttpMethod.Post, "/chats")
            {
                Content = new FormUrlEncodedContent(createChatCommandDict)
            };

            var response = await _unauthorizedClient.SendAsync(createChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
