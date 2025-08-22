using Domain.SharedKernel.Interfaces;
using Identity.Domain.Interfaces; 
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Api.ActionFilter;

public class UnitOfWorkActionFilter : IAsyncActionFilter
{
    private readonly IUnitOfWork _unitOfWork;
   // private readonly ILogger<UnitOfWorkActionFilter> _logger;

    public UnitOfWorkActionFilter(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
      //  _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpMethod = context.HttpContext.Request.Method;

      
        var resultContext = await next();

        // only commit changes for non-GET requests
        if (!string.Equals(httpMethod, "GET", StringComparison.OrdinalIgnoreCase) && resultContext.Exception == null)
        {
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
             //   _logger.LogError(ex, "Failed to commit UnitOfWork");
                throw; 
            }
        }
    }
}