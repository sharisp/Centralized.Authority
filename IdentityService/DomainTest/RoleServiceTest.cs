using Identity.Domain.Entity;
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
    public class RoleServiceTest
    {

        private readonly Mock<IRoleRepository> _roleRepository;
        private readonly Mock<ISystemConfigRepository> _sysConfResp;
        private readonly RoleDomainService roleDomainService;

        public RoleServiceTest()
        {
            _roleRepository = new Mock<IRoleRepository>();
            _sysConfResp = new Mock<ISystemConfigRepository>();
            roleDomainService = new RoleDomainService(_sysConfResp.Object, _roleRepository.Object);
        }
        [Fact]
        public async Task GetDefaultRoles_ShouldReturn_Null()
        {
            // Ar
            _sysConfResp.Setup(r => r.GetByConfigKey("DefaultRoles", null))
                      .ReturnsAsync((SystemConfig?)null);
            var roles = await roleDomainService.GetDefaultRolesAsync();
            Assert.Null(roles);
        }
    }
}
