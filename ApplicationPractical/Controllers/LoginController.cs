using ApplicationPractical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationPractical.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        public LoginController(JwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }
        
        
        [AllowAnonymous]
        [ActionName("Authorize")]
        [HttpPost]
        public IActionResult Authorize([FromBody] User usr)
        {
            var token = jwtAuthenticationManager.Authenticate(usr.Username, usr.Password);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            return Ok(token);
        }

        [HttpGet]
        public IActionResult TestRoute()
        {
            return Ok("Authorized");
        }
    }
}
