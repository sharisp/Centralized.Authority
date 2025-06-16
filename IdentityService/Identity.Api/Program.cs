
using Common.Jwt;
using FluentValidation;
using Identity.Api.Controllers;
using Identity.Api.MiddleWares;
using Identity.Domain.Events;
using Identity.Infrastructure;
using Identity.Infrastructure.EventHandler;
using System.Reflection;
using Identity.Api.Contracts.Validator;
using Identity.Api.ActionFilter;
using Identity.Domain.Interfaces;

namespace Identity.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // builder.Services.AddControllers();

          

          
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 
            builder.Services.AddSwagger_AuthSetup();
            builder.Services.AddJWTAuthentication(builder.Configuration);

          builder.Services.AddHttpContextAccessor();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
               

               /* cfg.RegisterServicesFromAssemblies(
                    typeof(LoginFailEventHandler).Assembly,  // Application
                    typeof(WeatherForecastController).Assembly     // Èç¹ûÓÐ Domain Event
                );*/
            });

            //  builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
           

            builder.Services.AddIdentityInfrastructure(builder.Configuration);
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<UnitOfWorkActionFilter>();
            builder.Services.AddControllers(options =>
            {
                options.Filters.AddService<UnitOfWorkActionFilter>();
            });
            var app = builder.Build();
            app.UseMiddleware<CustomerExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
