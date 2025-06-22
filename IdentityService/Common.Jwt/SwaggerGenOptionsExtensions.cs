using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Jwt;
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// internal method to add Swagger authentication setup, calling by AddJWTAuthentication
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal static void AddSwagger_AuthSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSwaggerGen(c =>
        {
            var scheme = new OpenApiSecurityScheme()
            {
                Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            };
            c.AddSecurityDefinition("Authorization", scheme);
            var requirement = new OpenApiSecurityRequirement();
            requirement[scheme] = new List<string>();
            c.AddSecurityRequirement(requirement);
        });



    }
}
