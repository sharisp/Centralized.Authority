using Domain.SharedKernel.HelperFunctions;
using Identity.Domain.Entity;
using Identity.Domain.ValueObject;
using IdGen;
using Moq;

namespace DomainTest
{
    [Collection("Global Test Collection")] //for multi unit test files
    public class UserUnitTest
    // public class UserUnitTest:IClassFixture<IdGeneratorFixture> //for single unit test file
    {

        [Fact]
        public void CreateUser_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var phone = new Identity.Domain.ValueObject.PhoneNumber("61", "123456");

            // Act
            var user = new User("test", "aa@aa.com", "123456", phone);

            // Assert
            Assert.NotNull(user);
            Assert.Equal("test", user.UserName);
            Assert.Equal("aa@aa.com", user.Email);
            Assert.Equal(HashHelper.ComputeMd5Hash("123456"), user.PasswordHash);
        }
        [Fact]
        public void PhoneNumber_ShouldBeImmutable()
        {
            var phone1 = new PhoneNumber("61", "123456");
            var phone2 = new PhoneNumber("61", "123456");

            Assert.Equal(phone1, phone2); // 值相等
            Assert.False(object.ReferenceEquals(phone1, phone2)); // 引用不同
        }



    }
}