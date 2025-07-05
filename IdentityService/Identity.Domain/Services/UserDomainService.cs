using Identity.Domain.Entity;
using Identity.Domain.Enums;

namespace Identity.Domain.Services
{
    public class UserDomainService(IUserRepository userRepository)
    {

        public async Task<(User?,LoginResult)> LoginByNameAndPwdAsync(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return (null,LoginResult.Fail);
            }

            var existUser = await userRepository.GetUseByNameAsync(userName);

            if (existUser == null)
            {
                return (null, LoginResult.UserNotFound);
            }
            if (existUser.AccessFail==null)
            {
                existUser.InitializeAccessFailIfNeeded();
            }
            if (!existUser.CheckPassword(passWord))
            {
                existUser.AccessFail.Fail();

                return (null, LoginResult.PasswordError);
            }
            if (existUser.AccessFail.CheckIfLocked())
            {
                existUser.AccessFail.Fail();
                return (null, LoginResult.UserLocked);
            }
           
            existUser.AccessFail.Reset();

            return (existUser, LoginResult.Success);
        }

        public async Task RegisterUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new ArgumentException("用户名或密码不能为空");
            }

            var existUser = await userRepository.GetUseByNameAsync(user.UserName);
            if (existUser != null)
            {
                throw new Exception("用户已存在");
            }

            await userRepository.AddUserAsync(user);


        }

      

     
    }
}
