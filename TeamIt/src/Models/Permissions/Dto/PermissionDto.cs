using AutoMapper;
using Models.Enums;
using Models.Mappings;

namespace Models.Permissions.Dto
{
    public class PermissionDto : IMapFrom<Domain.Entities.Permission>
    {
        public PermissionEnumDto Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Permission, PermissionDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(
                        permission => (PermissionEnumDto)permission.Id));
        }
    }
}