using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    public class UserDomainService(IUserRepository userRepository)
    {

        public async Task<bool> LoginByNameAndPwd(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                throw new ArgumentException("用户名或密码不能为空");
            }

            var existUser = await userRepository.GetUseByNameAsync(userName);

            if (existUser == null)
            {
                throw new Exception("用户名或者密码错误");
            }
            if (existUser.AccessFail.CheckIfLocked())
            {
                existUser.AccessFail.Fail();
                throw new Exception("已经锁定");
            }
            if (!existUser.CheckPassword(existUser.PasswordHash))
            {
                existUser.AccessFail.Fail();
                throw new Exception("密码错误");
            }
            existUser.AccessFail.Reset();


            return true;
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

        public void DeleteUser(User user)
        {
            
            userRepository.DeleteUser(user);
        }
    }
}
