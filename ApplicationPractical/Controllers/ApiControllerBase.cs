using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationPractical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private readonly IServiceProvider _ServiceProvider;
        public ApiControllerBase(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }
        protected T TryResolve<T>()
        {
            return _ServiceProvider.GetService<T>();
        }
        protected OkObjectResult ApiOk()
        {
            return Ok(new { Status = "success", MessageCode = "ok", MessageDetail = "" });
        }

        protected OkObjectResult ApiOk<T>(T data)
        {
            return Ok(new ApiResponse<T> { Status = "success", MessageCode = "ok", Data = data, MessageDetail = "" });
        }

        protected BadRequestObjectResult ApiError<T>(T data)
        {
            return BadRequest(new { Status = "false", MessageCode = "BadRequest", Data = data, MessageDetail = "" });
        }
    }
}
