using AutoMapper;
using Domain.Entities.ProjectManager;
using Models.Mappings;

namespace Models.Projects.Dto
{
    public class ProjectInfoDto : IMapFrom<Project>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public string? PictureUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectInfoDto>()
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        project => project.Picture.ImagePath));
        }
    }
}