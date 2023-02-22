using System.Net;

namespace Application.IntegrationTests.Chats.Commands
{
    public class EditChatCommandTests : TestsBase
    {
        private long _chatId;

        [OneTimeSetUp]
        public void OneTimeSetUp_EditChatCommandTests()
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
            context.ChatProfile.RemoveRange(context.ChatProfile);
            context.Chat.RemoveRange(context.Chat);
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
        public async Task ShouldEditChat()
        {
            var newName = "New name";
            var editChatCommandDict = new Dictionary<string, string>
            {
                { "ChatId", $"{_chatId}" },
                { "Name", newName },
            };
            var editChatCommand = new HttpRequestMessage(HttpMethod.Put, $"/chats/{_chatId}")
            {
                Content = new FormUrlEncodedContent(editChatCommandDict)
            };

            var response = await _client.SendAsync(editChatCommand);

            var context = GetDbContext();
            var chatDb = context.Chat.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chatDb.Name, Is.EqualTo(newName));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongChatId()
        {
            var newName = "New name";
            var editChatCommandDict = new Dictionary<string, string>
            {
                { "ChatId", "0" },
                { "Name", newName },
            };
            var editChatCommand = new HttpRequestMessage(HttpMethod.Put, "/chats/0")
            {
                Content = new FormUrlEncodedContent(editChatCommandDict)
            };

            var response = await _client.SendAsync(editChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var newName = "New name";
            var editChatCommandDict = new Dictionary<string, string>
            {
                { "ChatId", $"{_chatId}" },
                { "Name", newName },
            };
            var editChatCommand = new HttpRequestMessage(HttpMethod.Put, $"/chats/{_chatId}")
            {
                Content = new FormUrlEncodedContent(editChatCommandDict)
            };

            var response = await _unauthorizedClient.SendAsync(editChatCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}