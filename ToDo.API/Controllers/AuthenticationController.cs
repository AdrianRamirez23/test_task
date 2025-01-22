using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Interfaces;
using ToDo.Infraestructure.Dtos;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        public AuthenticationController(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid || loginDto == null)
            {
                return BadRequest(ModelState); 
            }

            if (loginDto.Username != "admin" || loginDto.Password != "password")
            {
                return Unauthorized("Invalid credentials.");
            }

            // Genera el token
            var token =  _tokenService.GetToken(loginDto.Username);

            return Ok(new { Token = token });
        }

    }
}
