using AutoMapper;
using Domain.Entities.ProjectManager;
using Models.Mappings;
using Models.Teams.Dto;

namespace Models.Projects.Dto
{
    public class ProjectMembersDto : IMapFrom<Project>
    {
        public ProjectInfoDto ProjectInfo { get; set; }
        public List<TeamMembersDto> Members { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectMembersDto>()
                .ForMember(
                    dest => dest.ProjectInfo,
                    opt => opt.MapFrom(
                        project => project))
                .ForMember(
                    dest => dest.Members,
                    opt => opt.MapFrom(
                        project => project.GetProjectTeamsList()));
        }
    }
}
