using Identity.Api.Contracts.Dtos.Request;
using Identity.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace Identity.Api.Contracts.Mapping
{
    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false)]
    public partial class MenuMapper: IMapperService
    {
        public partial CreateMenuDto ToDto(Menu info);
        public partial Menu ToEntity(CreateMenuDto infoDto);

        private void UpdateEntityFromDto(CreateMenuDto inDto, Menu outTarget)
        {
            if (!string.IsNullOrEmpty(inDto.SystemName))
            {
                outTarget.ChangeSystemName(inDto.SystemName);
            }
            if (inDto.ParentID>0&& inDto.ParentID!=outTarget.ParentID)
            {
                outTarget.ChangeParentID(inDto.ParentID);
            }
            if (!string.IsNullOrEmpty(inDto.Title))
            {
                outTarget.ChangeTitle(inDto.Title);
            }
            if (inDto.Sort > 0 && inDto.Sort != outTarget.Sort)
            {
                outTarget.ChangeSort(inDto.Sort);
            }
            if (!string.IsNullOrEmpty(inDto.Component))
            {
                outTarget.ChangeComponent(inDto.Component);
            }
            if (!string.IsNullOrEmpty(inDto.Path))
            {
                outTarget.ChangePath(inDto.Path);
            }
            if (inDto.Type >= 0 && inDto.Type != outTarget.Type)
            {
                outTarget.ChangeType(inDto.Type);
            }
        }

        [UserMapping(Default = true)]

        public void UpdateDtoToEntity(CreateMenuDto inDto, Menu outTarget)
        {
            UpdateEntityFromDto(inDto, outTarget);
        }
    }
}
