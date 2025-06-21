using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace Identity.Api.Contracts.Mapping
{
    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false, EnumMappingStrategy = EnumMappingStrategy.ByName)]

    public partial class UserMapper : IMapperService
    {

        // HAVE ORDER, input was the first parameter, output was the second parameter
        [MapProperty( nameof(User.PasswordHash), nameof(CreateUserRequestDto.Password))]
        public partial CreateUserRequestDto ToDto(User user);

        public partial UserResponseDto ToResponseDto(User user);

        [MapProperty(nameof(CreateUserRequestDto.Password), nameof(User.PasswordHash))]
        public partial User ToEntity(CreateUserRequestDto userDto);



        private void UpdateUserEntityFromDto(CreateUserRequestDto inDto, User outTarget)
        {
            if (!string.IsNullOrEmpty(inDto.Email))
            {
                outTarget.ChangeEmail(inDto.Email);
            }
            if (!string.IsNullOrEmpty(inDto.NickName))
            {
                outTarget.ChangeNickName(inDto.NickName);
            }
            if (!string.IsNullOrEmpty(inDto.UserName))
            {
                outTarget.ChangeUserName(inDto.UserName);
            }
            if (!string.IsNullOrEmpty(inDto.Password))
            {
                outTarget.ChangePassword(inDto.Password);
            }
        }

        [UserMapping(Default = true)]

        public  void UpdateDtoToEntity(CreateUserRequestDto inDto, User outTarget)
        {
            UpdateUserEntityFromDto(inDto, outTarget);
       
        }



    }
}