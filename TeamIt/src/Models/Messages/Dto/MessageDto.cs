using AutoMapper;
using Domain.Entities.Chats;
using Models.Mappings;

namespace Models.Messages.Dto
{
    public class MessageDto : IMapFrom<Message>
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        public string? SenderUserId { get; set; }

        public string? PictureUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, MessageDto>()
                .ForMember(
                    dest => dest.SenderUserId,
                    opt => opt.MapFrom(
                        message => message.SenderProfile == null ? null : message.SenderProfile.User.Id))
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        message => message.AttachedImage == null ? null : message.AttachedImage.ImagePath));
        }
    }
}
