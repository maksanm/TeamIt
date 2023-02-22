using Microsoft.AspNetCore.Http;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Chats;
using Models.Mappings;
using Models.Messages.Dto;
using Models.Users.Dto;

namespace Models.Chats.Dto
{
    public class ChatInfoDto : IMapFrom<Chat>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
        public MessageDto? LastMessage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Chat, ChatInfoDto>()
                .ForMember(
                    dest => dest.LastMessage,
                    opt => opt.MapFrom(
                        chat => chat.Messages.LastOrDefault()))
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        chat => chat.ChatPicture.ImagePath));
        }
    }
}
