using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Users.Commands;
using System.Security.Authentication;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IImageService _imageService;

        //empty constructor in mocking purposes
        public IdentityService()
        { }

        public IdentityService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            IImageService imageService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _imageService = imageService;
        }

        public async Task<string> CreateUserAsync(RegisterCommand registerCommand)
        {
            var user = new User
            {
                UserName = registerCommand.Login.Trim(),
                Name = registerCommand.Name.Trim(),
                Surname = registerCommand.Surname.Trim()
            };
            var registerResult = await _userManager.CreateAsync(user, registerCommand.Password);
            if (!registerResult.Succeeded)
                throw new InvalidCredentialException("Invalid user data provided");
            var token = _tokenService.CreateToken(user);
            if (registerCommand.Image is not null)
                await _imageService.SetUserProfileImage(user.Id, registerCommand.Image);
            return token;
        }

        public async Task<string> LoginUserWithUsernameAsync(LoginCommand loginCommand)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(loginCommand.Login, loginCommand.Password, false, false);
            if (!signInResult.Succeeded)
                throw new InvalidCredentialException("Incorrect login or password");
            var user = await _userManager.FindByNameAsync(loginCommand.Login);
            var token = _tokenService.CreateToken(user!);
            return token;
        }

        //virtual in mocking purposes
        public virtual async Task<User> GetCurrentUserAsync()
        {
            var currentUserClaimsPrincipal = _httpContextAccessor.HttpContext?.User;
            if (currentUserClaimsPrincipal is null)
                throw new UnauthorizedException();
            var currentUserIdentity = currentUserClaimsPrincipal.Identity;
            if (currentUserIdentity is null)
                throw new UnauthorizedException();
            return await _userManager.FindByNameAsync(currentUserIdentity.Name);
        }

        public async Task<TeamProfile> GetCurrentUserTeamProfileAsync(long teamId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();
            var profile = currentUser.TeamProfiles.FirstOrDefault(tp => tp.Team.Id == teamId);
            if (profile is null)
                throw new ValidationException("Current user is not a member of the team with provided id");
            return profile;
        }

        public async Task<ProjectProfile> GetCurrentUserProjectProfileAsync(long projectId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();
            var profile = currentUser.TeamProfiles
                .SelectMany(tp => tp.ProjectProfiles)
                .ToList()
                .FirstOrDefault(pp =>
                    pp.Project.Id == projectId);
            if (profile == default)
                throw new ValidationException("Current user is not a member of the project with provided id");
            return profile;
        }

        public async Task<ChatProfile> GetCurrentUserChatProfileAsync(long chatId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();
            var profile = currentUser.TeamProfiles
                .SelectMany(tp => tp.ProjectProfiles)
                .SelectMany(pp => pp.ChatProfiles)
                .Concat(currentUser.TeamProfiles
                    .SelectMany(tp => tp.ChatProfiles))
                .FirstOrDefault(cp =>
                    cp.Chat.Id == chatId &&
                    cp.User.Id == currentUser.Id);
            if (profile == default)
                throw new ValidationException("Current user is not a member of the chat with provided id");
            return profile;
        }
    }
}