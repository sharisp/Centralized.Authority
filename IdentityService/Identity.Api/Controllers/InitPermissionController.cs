using Domain.SharedKernel.Interfaces;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Constants;
using Identity.Domain.Entity;
using Identity.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitPermissionController(IUnitOfWork unitOfWork, BaseDbContext baseDbContext, PermissionRepository permissionRepository) : ControllerBase
    {

        //be careful, this is only for init permissions, should not be used in production environment
       
       // [Authorize]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<string>>> InitPermissions()
        {
            var role = await baseDbContext.Roles.FirstOrDefaultAsync(t => t.RoleName == "adminRole");
            if (role == null)
            {
                role = new Domain.Entity.Role("adminRole");
                await baseDbContext.Roles.AddAsync(role);
            }

            var user = await baseDbContext.Users.FirstOrDefaultAsync(t => t.UserName == "admin");
            if (user == null)
            {
                user = new Domain.Entity.User("admin", "admin", "aa@aa.com");
                user.AddRole(role);
                await baseDbContext.Users.AddAsync(user);
            }

            var listPermissions = new List<Permission>();
            var permissionKeys = PermissionScanner.GetAllPermissionKeys(Assembly.GetExecutingAssembly());
            var realrolePermission = baseDbContext.Roles.Include(t => t.Permissions).FirstOrDefault(t => t.RoleName == "adminRole")?.Permissions.ToList() ?? new List<Permission>();
            foreach (var permissionKey in permissionKeys)
            {
                var permission = await permissionRepository.GetByPermissionKeyAsync(permissionKey, ConstantValues.SystemName);
                if (permission == null)
                {
                    permission = new Permission(permissionKey, ConstantValues.SystemName, permissionKey);

                    await permissionRepository.AddAsync(permission);

                }
                // can not use role.Permissions,not include
                // ensure the permission is loaded
                if (!realrolePermission.Any(p => p.PermissionKey == permission.PermissionKey && p.SystemName == permission.SystemName))
                {
                    listPermissions.Add(permission);

                }
               
            }
            if (listPermissions.Any())
            {
                role.AddPermissions(listPermissions);
            }


            await unitOfWork.SaveChangesAsync();

            return this.OkResponse("init successfully");
        }
    }
}