using AutoMapper;
using Domain.Entities;
using Models.Enums;
using Models.Mappings;
using Models.Users.Dto;

namespace Models.Tasks.Dto
{
    public class TaskDto : IMapFrom<Domain.Entities.ProjectManager.Task>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public TaskStateEnumDto State { get; set; }
        public UserDto? AssignedUser { get; set; }
        public List<TaskDto> Subtasks { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ProjectManager.Task, TaskDto>()
                .ForMember(
                    dest => dest.AssignedUser,
                    opt => opt.MapFrom(
                        task => task.AssigneeProfile.User));
        }
    }
}