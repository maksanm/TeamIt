using System.Net.Http.Json;
using System.Net;
using Models.Chats.Dto;

namespace Application.IntegrationTests.Chats.Queries
{
    public class GetChatByIdQueryTests : TestsBase
    {
        private long _chatId;

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
                var chat = CreateChat(_currentUserId, userId, team, null, "Chat 1");
                context.Chat.Add(chat);
                context.SaveChanges();
                _chatId = chat.Id;
            }
        }

        [Test]
        public async Task ShouldGetProjectById()
        {
            var response = await _client.GetAsync($"/chats/{_chatId}");
            var chatDto = await response.Content.ReadFromJsonAsync<ChatDto>();

            var context = GetDbContext();
            var chatDb = context.Chat.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chatDb.Id, Is.EqualTo(chatDto.Id));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var response = await _client.GetAsync("/chats/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/chats/{_chatId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
