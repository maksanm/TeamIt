using AutoMapper;
using Models.Enums;
using Models.Mappings;
using Models.Users.Dto;

namespace Models.Tasks.Dto
{
    public class TaskInfoDto : IMapFrom<Domain.Entities.ProjectManager.Task>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public TaskStateEnumDto State { get; set; }
        public TaskInfoDto? ParentTaskInfo { get; set; }
        public UserDto AssignedUser { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ProjectManager.Task, TaskInfoDto>()
                .ForMember(
                    dest => dest.AssignedUser,
                    opt => opt.MapFrom(
                        task => task.AssigneeProfile.User))
                .ForMember(
                    dest => dest.ParentTaskInfo,
                    opt => opt.MapFrom(
                        task => task.ParentTask));
        }
    }
}