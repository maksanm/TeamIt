using AutoMapper;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Models.Mappings;
using Models.Roles.Dto;

namespace Models.Projects.Dto
{
    public class ProjectDto : IMapFrom<Project>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public RoleDto LimitRole { get; set; }
        public bool UseOwnHierarchy { get; set; }
        public long CreatorTeamId { get; set; }
        public string? PictureUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectDto>()
                .ForMember(
                    dest => dest.PictureUrl,
                    opt => opt.MapFrom(
                        user => user.Picture.ImagePath));
        }
    }
}