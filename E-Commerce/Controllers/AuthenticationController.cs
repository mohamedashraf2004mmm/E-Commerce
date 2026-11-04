using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        //check email exist
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>>CheckEmail([FromQuery]string email , CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.CheckEmailExistAsync(email, ct));
        }
        //get current user
        [HttpGet("Profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("Token does not have email claim"); //has all info of authenticated user
            return ToActionResult(await _authenticationService.GetCurrentUserAsync(email, ct));

        }

        //get current user address

        //update current user address

    }
}
