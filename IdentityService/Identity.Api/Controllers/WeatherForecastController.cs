using Identity.Domain.Entity;
using Identity.Domain.Events;
using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Identity.Domain.ValueObject;
using Identity.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator mediator;
        private readonly UserDomainService userDomainServiceService;
        private readonly BaseDbContext dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator, BaseDbContext dbContext)
        {
            _logger = logger;
            this.mediator = mediator;
            this.dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public  IEnumerable<WeatherForecast> Get()
        {
            var userInfo = new User("uname","123456",new PhoneNumber("86","15619311120"),"619214460@qq.com",null,null,null) ;
           mediator.Publish(new LoginFailEvent(userInfo));
        //  await  userDomainServiceService.LoginByNameAndPwd("11","1111");
            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("Test")]
        public async  Task<int> Test()
        {
            var userInfo = new User("uname", "123456", new PhoneNumber("86", "15619311120"), "619214460@qq.com", null, null, null);
            //   mediator.Publish(new LoginFailEvent(userInfo));
            await dbContext.Users.AddAsync(userInfo);
            await dbContext.SaveChangesAsync();

            return 1;
        }
    }
}
