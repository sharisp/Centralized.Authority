using Identity.Domain.Entity;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    [Collection("Global Test Collection")]
    public class UserServieTest
    {


        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserDomainService _userDomainService;

        public UserServieTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userDomainService = new UserDomainService(_userRepoMock.Object);
        }

        [Fact]
        public async Task LoginByNameAndPwdAsync_Should_Return_UserNotFound_When_User_Not_Exist()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetUserWithRolesByNameAsync("test"))
                         .ReturnsAsync((User)null);

            // Act
            var (user, result) = await _userDomainService.LoginByNameAndPwdAsync("test", "123456");

            // Assert
            Assert.Null(user);
            Assert.Equal(LoginResult.UserNotFound, result);
            _userRepoMock.Verify(r => r.GetUserWithRolesByNameAsync("test"), Times.Once);
        }

        [Fact]
        public async Task LoginByNameAndPwdAsync_Should_Return_PasswordError_When_Password_Incorrect()
        {
            // Arrange
            var existingUser = new User("aa", "11@11.com", "123"); // 正确密码是 123
            _userRepoMock.Setup(r => r.GetUserWithRolesByNameAsync("aa"))
                         .ReturnsAsync(existingUser);

            // Act
            var (user, result) = await _userDomainService.LoginByNameAndPwdAsync("aa", "wrongpwd");

            // Assert
            Assert.Null(user);
            Assert.Equal(LoginResult.PasswordError, result);
        }

        [Fact]
        public async Task LoginByNameAndPwdAsync_Should_Return_Success_When_Password_Correct()
        {
            // Arrange
            var existingUser = new User("aa", "11@11.com", "123");
            _userRepoMock.Setup(r => r.GetUserWithRolesByNameAsync("aa"))
                         .ReturnsAsync(existingUser);

            // Act
            var (user, result) = await _userDomainService.LoginByNameAndPwdAsync("aa", "123");

            // Assert
            Assert.NotNull(user);
            Assert.Equal(LoginResult.Success, result);
        }


        [Fact]
        public async Task LoginByNameAndPwdAsync_Should_Handle_Different_User_Cases()
        {
            // Arrange
            //    var mockUserRepo = new Mock<IUserRepository>();

          
            _userRepoMock.Setup(r => r.GetUserWithRolesByNameAsync(It.Is<string>(x => x == "normalUser")))
                        .ReturnsAsync(new User("normalUser", "user@test.com", "123456"));

         
            var lockedUser = new User("lockedUser", "locked@test.com", "123456");

            lockedUser.AccessFail.Fail(); // 模拟失败次数达到锁定条件
            lockedUser.AccessFail.Fail();
            lockedUser.AccessFail.Fail();

            _userRepoMock.Setup(r => r.GetUserWithRolesByNameAsync(It.Is<string>(x => x == "lockedUser")))
                        .ReturnsAsync(lockedUser);

           
            _userRepoMock.Setup(r => r.GetUserWithRolesByNameAsync(It.Is<string>(x => x == "notfound")))
                        .ReturnsAsync((User)null);

            //  var userDomainService = new UserDomainService(mockUserRepo.Object);

            // Act & Assert

            // ✅ Case 1: normal user
            var (user1, result1) = await _userDomainService.LoginByNameAndPwdAsync("normalUser", "123456");
            Assert.Equal(LoginResult.Success, result1);
            Assert.NotNull(user1);

            // ✅ Case 2: locked user
            var (user2, result2) = await _userDomainService.LoginByNameAndPwdAsync("lockedUser", "123456");
            Assert.Equal(LoginResult.UserLocked, result2);

            // ✅ Case 3: user not exist
            var (user3, result3) = await _userDomainService.LoginByNameAndPwdAsync("notfound", "whatever");
            Assert.Equal(LoginResult.UserNotFound, result3);
        }
    }
}



