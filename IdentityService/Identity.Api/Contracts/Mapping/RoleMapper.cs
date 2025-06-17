using Identity.Api.Contracts.Dtos.Request;
using Identity.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace Identity.Api.Contracts.Mapping
{
    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false)]
    public partial class RoleMapper : IMapperService
    {
        public partial CreateRoleDto ToDto(Role role);
        public partial Role ToEntity(CreateRoleDto roleDto);
        public partial void UpdateDtoToRole(CreateRoleDto inDto, Role outTarget);

    }
}
