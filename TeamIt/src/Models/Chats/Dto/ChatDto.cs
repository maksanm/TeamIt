using Microsoft.AspNetCore.Http;
using AutoMapper;
using Domain.Entities.Chats;
using Models.Mappings;
using Models.Users.Dto;

namespace Models.Chats.Dto
{
    public class ChatDto : IMapFrom<Chat>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
        public List<UserDto> ChatMembers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Chat, ChatDto>()
                .ForMember(
                    dest => dest.ChatMembers,
                    opt => opt.MapFrom(
                        chat => chat.Profiles.Select(p => p.User)))
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        chat => chat.ChatPicture.ImagePath));
        }
    }
}
