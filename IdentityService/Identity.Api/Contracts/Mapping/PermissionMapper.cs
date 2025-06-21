using Identity.Api.Contracts.Dtos.Request;
using Identity.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace Identity.Api.Contracts.Mapping
{
    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false)]
    public partial class PermissionMapper: IMapperService
    {
        public partial CreatePermissionDto ToDto(Permission info);
        public partial Permission ToEntity(CreatePermissionDto infoDto);

        private void UpdateEntityFromDto(CreatePermissionDto inDto, Permission outTarget)
        {
            if (!string.IsNullOrEmpty(inDto.SystemName))
            {
                outTarget.ChangeSystemName(inDto.SystemName);
            }
            if (!string.IsNullOrEmpty(inDto.PermissionKey))
            {
                outTarget.ChangePermissionKey(inDto.PermissionKey);
            }
            if (!string.IsNullOrEmpty(inDto.Title))
            {
                outTarget.ChangeTitle(inDto.Title);
            }
         
        }

        [UserMapping(Default = true)]

        public void UpdateDtoToEntity(CreatePermissionDto inDto, Permission outTarget)
        {
            UpdateEntityFromDto(inDto, outTarget);
        }
    }
}
