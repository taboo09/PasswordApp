using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PassApp.API.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace PassApp.API.Helper
{

    public class LogUsersActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContext.HttpContext.RequestServices.GetService<IAppRepository>();
            var user = await repo.GetUser(userId);

            // display logged on time

            await repo.SaveAll();
        }


    }

}