
using ApiAuth.Api.Middles;
using Common.Jwt;
using FluentValidation;
using Identity.Api.ActionFilter;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Api.Controllers;
using Identity.Api.MiddleWares;
using Identity.Domain.Interfaces;
using Identity.Infrastructure;
using Identity.Infrastructure.EventHandler;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
            builder.Services.AddJWTAuthentication(builder.Configuration);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMediatR(cfg =>
            {
              //  cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());


                 cfg.RegisterServicesFromAssemblies(
                     typeof(LoginFailEventHandler).Assembly,  // Application
                     typeof(UserController).Assembly     // Èç¹ûÓÐ Domain Event
                 );
            });

            //  builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();


            builder.Services.AddIdentityInfrastructure(builder.Configuration);
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<UnitOfWorkActionFilter>();

            builder.Services.AddAllMapper();
            builder.Services.AddControllers(options =>
            {
                options.Filters.AddService<UnitOfWorkActionFilter>();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>

                     new BadRequestObjectResult(ApiResponse<string>.Fail("param error"));
            }).AddJsonOptions(options =>
            {
                //configure json options,long type bug
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
                    | System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            }); ;
            var app = builder.Build();
            app.UseMiddleware<CustomerExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
                app.Urls.Add($"http://*:{port}");

                app.MapGet("/", () => "Hello from DotnetCore!");
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseMiddleware<CustomJwtAuthMiddleware>();
            app.Run();
        }
    }
}
