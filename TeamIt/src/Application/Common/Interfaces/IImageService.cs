using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<string> SetUserProfileImage(string userId, IFormFile file);
        Task<string> SetTeamPicture(long teamId, IFormFile file);
        Task<string> SetProjectPicture(long projectId, IFormFile file);
        Task<string> SetChatPicture(long chatId, IFormFile file);
        Task<string> AddMessageImage(long messageId, IFormFile file);
    }
}
