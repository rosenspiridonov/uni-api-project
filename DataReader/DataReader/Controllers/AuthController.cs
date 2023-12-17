using DataReader.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataReader.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(string username, string password)
        {
            var result = await _authService.RegisterAsync(username, password);
            if (result)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest("Registration failed.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var token = await _authService.LoginAsync(username, password);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }
        }
    }
}
