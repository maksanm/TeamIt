using MediatR;
using Models.Users.Dto;

namespace Models.Users.Queries
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
    }
}