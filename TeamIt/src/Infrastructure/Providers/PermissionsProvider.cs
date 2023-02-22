using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Providers
{
    public class PermissionsProvider : IPermissionsProvider
    {
        private readonly IApplicationDbContext _context;

        public PermissionsProvider(IApplicationDbContext context)
        {
            _context = context;
        }

        public List<Permission> PermissionsWithIds(List<int> ids) =>
            _context.Permission
                .Where(p => ids.Contains((int)p.Id))
                .ToList();

        public List<Permission> AllPermissions() => 
            _context.Permission
                .ToList();
    }
}