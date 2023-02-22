using AutoMapper;
using Domain.Entities.Teams;
using Models.Mappings;
using Models.Users.Dto;

namespace Models.Teams.Dto
{
    public class JoinTeamRequestDto : IMapFrom<JoinTeamRequest>
    {
        public long Id { get; set; }
        public TeamDto Team { get; set; }
        public UserDto RequestSender { get; set; }
    }
}
