using CorePackages.Security.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected string? getIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        protected int getUserIdFromRequest() //todo authentication behavior?
        {
            int userId = HttpContext.User.GetUserId();
            return userId;
        }

    }
}
