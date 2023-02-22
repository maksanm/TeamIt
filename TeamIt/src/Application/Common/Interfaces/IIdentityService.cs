using Domain.Entities;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Models.Users.Commands;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(RegisterCommand registerCommand);

        Task<string> LoginUserWithUsernameAsync(LoginCommand loginCommand);

        Task<User> GetCurrentUserAsync();

        Task<TeamProfile> GetCurrentUserTeamProfileAsync(long teamId);

        Task<ProjectProfile> GetCurrentUserProjectProfileAsync(long projectId);

        Task<ChatProfile> GetCurrentUserChatProfileAsync(long chatId);
    }
}