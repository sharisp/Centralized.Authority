using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace Identity.Api.Contracts.Mapping
{
    [Mapper(AllowNullPropertyAssignment=false, ThrowOnPropertyMappingNullMismatch = false)]
    public  partial class UserMapper : IMapperService
    {
        public partial CreateUserRequestDto ToDto(User user);
        public partial UserResponseDto ToResponseDto(User user);
        public partial User ToEntity(CreateUserRequestDto userDto);

        public  partial void UpdateDtoToEntity(CreateUserRequestDto inDto, User outTarget);
    }
}
