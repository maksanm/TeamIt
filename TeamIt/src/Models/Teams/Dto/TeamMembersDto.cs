using AutoMapper;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Models.Mappings;
using Models.Projects.Dto;

namespace Models.Teams.Dto
{
    public class TeamMembersDto : IMapFrom<Team>
    {
        public TeamDto TeamInfo { get; set; }
        public List<TeamMemberDto> Members { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Team, TeamMembersDto>()
                .ForMember(
                    dest => dest.TeamInfo,
                    opt => opt.MapFrom(
                        team => team))
                .ForMember(
                    dest => dest.Members,
                    opt => opt.MapFrom(
                        team => team.Profiles));
        }
    }
}