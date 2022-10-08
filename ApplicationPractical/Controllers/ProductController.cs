using ApplicationPractical.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationPractical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {
        protected readonly IProductService _IProductService;
        public ProductController(IServiceProvider pServiceProvider) : base(pServiceProvider)
        {
          _IProductService = TryResolve<IProductService>();
        }
        [AllowAnonymous]
        [ActionName("Authorize")]
        [HttpGet]
        public async Task<IActionResult> Authorize()
        {
            var test = await _IProductService.GetProductByName(1);
            return ApiOk("hello");
        }
    }
}
