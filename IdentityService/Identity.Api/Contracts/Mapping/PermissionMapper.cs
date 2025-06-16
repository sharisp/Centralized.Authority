using ApiAuth.Entities.DTO;
using Identity.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace Identity.Api.Contracts.Mapping
{
    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false)]
    public partial class PermissionMapper: IMapperService
    {
        public partial CreatePermissionDto ToDto(Permission info);
        public partial Permission ToEntity(CreatePermissionDto infoDto);

        public partial void UpdateDtoToEntity(CreatePermissionDto inDto, Permission outTarget);
    }
}
