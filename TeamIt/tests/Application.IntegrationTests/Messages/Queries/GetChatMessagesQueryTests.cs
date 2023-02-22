using Models.Chats.Dto;
using System.Net.Http.Json;
using System.Net;
using Models.Messages.Dto;

namespace Application.IntegrationTests.Messages.Queries
{
    public class GetChatMessagesQueryTests : TestsBase
    {
        private long _chatId;
        private int _messageLimit = 5;
        private int _messagesCount = 5;

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
                var chat = CreateChat(_currentUserId, userId, team, null, "Chat 1", _messagesCount);
                context.Chat.Add(chat);
                context.SaveChanges();
                _chatId = chat.Id;
            }
        }

        [Test]
        public async Task ShouldGetChatMessages()
        {
            var response = await _client.GetAsync($"/chats/{_chatId}/messages/{_messageLimit}");
            var messageDtos = await response.Content.ReadFromJsonAsync<List<MessageDto>>();

            var context = GetDbContext();
            var chatDb = context.Chat.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chatDb.Messages.Count, Is.EqualTo(messageDtos.Count));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongChatId()
        {
            var response = await _client.GetAsync($"/chats/0/messages/{_messageLimit}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        
        [Test]
        public async Task ShouldReturnBadRequest_ForNegativeLimit()
        {
            var response = await _client.GetAsync($"/chats/{_chatId}/messages/-1");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/chats/{_chatId}/messages/{_messageLimit}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
