
using ApiAuth.Api.Middles;
using Common.Jwt;
using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api.ActionFilter;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Api.Controllers;
using Identity.Api.MiddleWares;
using Identity.Infrastructure;
using Identity.Infrastructure.EventHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Identity.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddJWTAuthentication(builder.Configuration);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMediatR(cfg =>
            {
              //  cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());


                 cfg.RegisterServicesFromAssemblies(
                     typeof(LoginFailEventHandler).Assembly,  // Application
                     typeof(UserController).Assembly     // 如果有 Domain Event
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

                     new BadRequestObjectResult(ApiResponse<BaseResponse>.Fail("param error"));
            }).AddJsonOptions(options =>
            {
                //configure json options,long type bug
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
                    | System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
                // 忽略循环引用（防止对象环导致异常）
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }); 
            var app = builder.Build();

            app.UseCors("AllowAll");
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
                
            }

            app.MapGet("/", [AllowAnonymous] () => "Hello from DotnetCore!");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseMiddleware<CustomJwtAuthMiddleware>();
            app.Run();
        }
    }
}
