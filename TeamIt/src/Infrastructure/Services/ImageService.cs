using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.IO;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        private readonly long _maxImageSize = 15 * 1024 * 1024; //15MB

        public ImageService(
            IApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> SetUserProfileImage(string userId, IFormFile file)
        {
            var user = await _context.User.FindAsync(userId);
            if (user is null)
                throw new ValidationException("User with provided id does not exist");

            if (user.Image is not null)
                DeletePicture(user.Image);

            var path = await SaveJpegImageToRootFolder("users", file);

            var profilePicture = new Picture() { ImagePath = path };
            user.Image = profilePicture;

            _context.Picture.Add(profilePicture);
            await _context.SaveChangesAsync(CancellationToken.None);

            return path;
        }

        public async Task<string> SetTeamPicture(long teamId, IFormFile file)
        {
            var team = await _context.Team.FindAsync(teamId);
            if (team is null)
                throw new ValidationException("Team with provided id does not exist");

            if (team.Picture is not null)
                DeletePicture(team.Picture);

            var path = await SaveJpegImageToRootFolder("teams", file);

            var teamPicture = new Picture() { ImagePath = path };
            team.Picture = teamPicture;

            _context.Picture.Add(teamPicture);
            await _context.SaveChangesAsync(CancellationToken.None);

            return path;
        }

        public async Task<string> SetProjectPicture(long projectId, IFormFile file)
        {
            var project = await _context.Project.FindAsync(projectId);
            if (project is null)
                throw new ValidationException("Project with provided id does not exist");

            if (project.Picture is not null)
                DeletePicture(project.Picture);

            var path = await SaveJpegImageToRootFolder("projects", file);

            var projectPicture = new Picture() { ImagePath = path };
            project.Picture = projectPicture;

            _context.Picture.Add(projectPicture);
            await _context.SaveChangesAsync(CancellationToken.None);

            return path;
        }

        public async Task<string> SetChatPicture(long chatId, IFormFile file)
        {
            var chat = await _context.Chat.FindAsync(chatId);
            if (chat is null)
                throw new ValidationException("Chat with provided id does not exist");

            if (chat.ChatPicture is not null)
                DeletePicture(chat.ChatPicture);

            var path = await SaveJpegImageToRootFolder("chats", file);

            var chatPicture = new Picture() { ImagePath = path };
            chat.ChatPicture = chatPicture;

            _context.Picture.Add(chatPicture);
            await _context.SaveChangesAsync(CancellationToken.None);

            return path;
        }

        public async Task<string> AddMessageImage(long messageId, IFormFile file)
        {
            var message = await _context.Message.FindAsync(messageId);
            if (message is null)
                throw new ValidationException("Message with provided id does not exist");

            var path = await SaveJpegImageToRootFolder("messages", file);

            var attachedImagePicture = new Picture() { ImagePath = path };
            message.AttachedImage = attachedImagePicture;

            _context.Picture.Add(attachedImagePicture);
            await _context.SaveChangesAsync(CancellationToken.None);

            return path;
        }

        private void DeletePicture(Picture picture)
        {
            File.Delete(_environment.ContentRootPath + "\\wwwroot" + picture.ImagePath);
            _context.Picture.Remove(picture);
        }

        private async Task<string> SaveJpegImageToRootFolder(string folderName, IFormFile file)
        {
            string trustedFileNameForFileStorage;
            if (file.Length == 0)
            {
                throw new ValidationException("Image file bytes array is empty");
            }
            else if (file.Length > _maxImageSize)
            {
                throw new ValidationException("Image is too large");
            }
            else
            {
                try
                {
                    trustedFileNameForFileStorage = Guid.NewGuid().ToString() + ".jpg";

                    await using MemoryStream stream = new();
                    await file.CopyToAsync(stream);
                    Image image = new Bitmap(stream);
                    var imagePath = Path.Combine("\\images", folderName, trustedFileNameForFileStorage);
                    image.Save(_environment.ContentRootPath + "\\wwwroot" + imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return imagePath;
                }
                catch (IOException ex)
                {
                    throw new CustomApplicationException("Image upload error: " + ex.Message);
                }
            }
        }
    }
}
