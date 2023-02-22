using System.Net;

namespace Application.IntegrationTests.Chats.Commands
{
    public class DeleteChatCommandTests : TestsBase
    {
        private long _chatId;

        [OneTimeSetUp]
        public void OneTimeSetUp_DeleteChatCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam(otherMembersCount: 2);
            context.Team.Add(team);
            context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (context.Chat.Any())
                return;
            var team = context.Team.First();
            var userId = team.Profiles
                .Select(p => p.UserId)
                .FirstOrDefault(id => id != _currentUserId);
            var chat = CreateChat(_currentUserId, userId, team, null);
            context.Chat.Add(chat);
            context.SaveChanges();
            _chatId = chat.Id;
        }

        [Test]
        public async Task ShouldDeleteChat()
        {
            var response = await _client.DeleteAsync($"/chats/{_chatId}");

            var context = GetDbContext();
            var chatCountDb = context.Chat.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chatCountDb, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongChatId()
        {
            var response = await _client.DeleteAsync("/chats/0");

            var context = GetDbContext();
            var chatCountDb = context.Chat.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(chatCountDb, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync($"/chats/{_chatId}");

            var context = GetDbContext();
            var chatCountDb = context.Chat.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(chatCountDb, Is.EqualTo(1));
        }
    }
}
