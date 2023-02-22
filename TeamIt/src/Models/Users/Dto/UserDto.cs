using AutoMapper;
using Domain.Entities;
using Models.Mappings;

namespace Models.Users.Dto
{
    public class UserDto : IMapFrom<User>
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ImageUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Login,
                    opt => opt.MapFrom(
                        user => user.UserName))
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(
                        user => user.Image.ImagePath));
        }
    }
}