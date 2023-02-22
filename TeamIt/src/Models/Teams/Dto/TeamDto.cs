using AutoMapper;
using Domain.Entities;
using Domain.Entities.Teams;
using Models.Mappings;
using Models.Users.Dto;

namespace Models.Teams.Dto
{
    public class TeamDto : IMapFrom<Team>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CreatorUserId { get; set; }
        public string? PictureUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Team, TeamDto>()
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        user => user.Picture.ImagePath));
        }
    }
}