using Models.Chats.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Chats.Commands
{
    public class AddUserToChatCommandTests : TestsBase
    {
        private long _chatId;
        private string _userId;

        [OneTimeSetUp]
        public void OneTimeSetUp_EditChatCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam(otherMembersCount: 3);
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
            var otherUsers = team.Profiles
                .Select(p => p.UserId)
                .Where(id => id != _currentUserId);
            var chat = CreateChat(_currentUserId, otherUsers.ElementAt(0), team, null);
            context.Chat.Add(chat);
            context.SaveChanges();
            _chatId = chat.Id;
            _userId = otherUsers.ElementAt(1);
        }

        [Test]
        public async Task ShouldAddChatMember()
        {
            var addChatMemberCommand = new AddUserToChatCommand()
            {
                ChatId= _chatId,
                UserId= _userId
            };

            var response = await _client.PostAsJsonAsync($"/chats/{_chatId}/users", addChatMemberCommand);

            var context = GetDbContext();
            var chatMembersCountDb = context.Chat.First().Profiles.Count;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(chatMembersCountDb, Is.EqualTo(3));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongChatId()
        {
            var addChatMemberCommand = new AddUserToChatCommand()
            {
                ChatId = 0,
                UserId = _userId
            };

            var response = await _client.PostAsJsonAsync("/chats/0/users", addChatMemberCommand);

            var context = GetDbContext();
            var chatMembersCountDb = context.Chat.First().Profiles.Count;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(chatMembersCountDb, Is.EqualTo(2));
        }
        
        [Test]
        public async Task ShouldReturnBadRequest_ForWrongUserId()
        {
            var addChatMemberCommand = new AddUserToChatCommand()
            {
                ChatId = _chatId,
                UserId = "blablabla"
            };

            var response = await _client.PostAsJsonAsync($"/chats/{_chatId}/users", addChatMemberCommand);

            var context = GetDbContext();
            var chatMembersCountDb = context.Chat.First().Profiles.Count;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(chatMembersCountDb, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var addChatMemberCommand = new AddUserToChatCommand()
            {
                ChatId = _chatId,
                UserId = "blablabla"
            };

            var response = await _unauthorizedClient.PostAsJsonAsync($"/chats/{_chatId}/users", addChatMemberCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
