using Identity.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Identity.Api
{
    public class PermissionScanner
    {
        public static List<string> GetAllPermissionKeys(Assembly assembly)
        {
            var permissions = new List<string>();

            var controllerTypes = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)) || t.GetCustomAttributes<ApiControllerAttribute>().Any());

            foreach (var controller in controllerTypes)
            {
                // 类级特性
                var classAttributes = controller.GetCustomAttributes<PermissionKeyAttribute>();
                permissions.AddRange(classAttributes.Select(attr => attr.Key));

                // 方法级特性
                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    var methodAttributes = method.GetCustomAttributes<PermissionKeyAttribute>();
                    permissions.AddRange(methodAttributes.Select(attr => attr.Key));
                }
            }

            return permissions.Distinct().ToList();
        }
    }

}
