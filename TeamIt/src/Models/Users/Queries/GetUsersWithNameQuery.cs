using MediatR;
using Models.Users.Dto;

namespace Models.Users.Queries
{
    public class GetUsersWithNameQuery : IRequest<IList<UserDto>>
    {
        public string Name { get; set; }
    }
}