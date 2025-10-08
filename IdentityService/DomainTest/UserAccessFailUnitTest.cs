using Identity.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    [Collection("Global Test Collection")]
    public class UserAccessFailUnitTest
    {
        [Fact]
        public void TestAccess_Fail_Lock()
        {
            var user = new User("test", "aa@aa.com", "123456", null);

            var access=new UserAccessFail(user);
         
            Assert.NotNull(access);
            Assert.Null(access.LockEndTime);

            access.Fail();
            Assert.Null(access.LockEndTime);
            access.Fail();
            Assert.Null(access.LockEndTime);
            access.Fail();
            Assert.NotNull(access.LockEndTime);
          
        }

        [Fact]
        public void TestAccess_Fail_Reset()
        {
            var user = new User("test", "aa@aa.com", "123456", null);

            var access = new UserAccessFail(user);


            access.Fail();           
            access.Fail();
            access.Fail();
            Assert.NotNull(access.LockEndTime);
            Assert.True(access.FailCount == 3);

            access.Fail();
            Assert.NotNull(access.LockEndTime);
            Assert.True(access.FailCount == 4);
            access.Reset();
            Assert.Null(access.LockEndTime);
            Assert.True(access.FailCount == 0);

        }
    }
}
