using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Users.Dto;
using Models.Users.Queries;
using System.Xml.Linq;

namespace Application.Handlers.Users.Queries
{
    public class GetUsersWithNameQueryHandler : IRequestHandler<GetUsersWithNameQuery, IList<UserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersWithNameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<UserDto>> Handle(GetUsersWithNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException();
            var users = await _context.User
                .Where(user =>
                    user.UserName!.ToLower().Contains(request.Name)
                    || user.Name!.ToLower().Contains(request.Name)
                    || user.Surname!.ToLower().Contains(request.Name))
                .ToListAsync();

            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }
    }
}