using System.Net.Http.Json;
using System.Net;
using Domain.Entities.Chats;
using Domain.Entities.Teams;
using Models.Chats.Dto;

namespace Application.IntegrationTests.Chats.Queries
{
    public class GetCurrentUserChatsQueryTests : TestsBase
    {
        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (!context.Team.Any())
            {
                var team = CreateTeam(otherMembersCount: 1);
                context.Team.Add(team);
                context.SaveChanges();
                var userId = team.Profiles
                    .Select(p => p.UserId)
                    .FirstOrDefault(id => id != _currentUserId);
                context.Chat.AddRange(CreateChats(userId, team));
            }
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldReturnCurrentChatInfos()
        {
            var response = await _client.GetAsync("/chats");
            var chatDtos = await response.Content.ReadFromJsonAsync<List<ChatInfoDto>>();

            var context = GetDbContext();
            var chatsFromDb = context.Chat.ToList();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chatsFromDb.Count, Is.EqualTo(chatDtos.Count));
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync("/chats");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private List<Chat> CreateChats(string userId, Team baseTeam) => new List<Chat>()
        {
            CreateChat(_currentUserId, userId, baseTeam, null, "Chat 1"),
            CreateChat(_currentUserId, userId, baseTeam, null, "Chat 2"),
            CreateChat(_currentUserId, userId, baseTeam, null, "Chat 3")
        };
    }
}
