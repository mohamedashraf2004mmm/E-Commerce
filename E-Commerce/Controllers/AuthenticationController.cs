using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = E_Commerce.Application.Contracts.IAuthenticationService;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        //Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto , CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.LoginAsync(loginDto , ct));
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto , CancellationToken ct)
        {
          return ToActionResult(await _authenticationService.RegisterAsync(registerDto , ct));
        }
    }
}
