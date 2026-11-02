using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class MyAuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public MyAuthenticationService(IIdentityService identityService , ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            //get user by email
            var userResult = await _identityService.FindUserByEmailAsync(loginDto.Email);
            if (!userResult.IsSuccess)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }

            //check password
            var passwordResult = await _identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password);
            if (!passwordResult.IsSuccess)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }
            if (!passwordResult.data)
            {
                return Result<UserDto>.Fail(Error.Unauthorized("Invalid email or password"));
            }

            var user = userResult.data;
            var userRoles = await _identityService.GetUserRoles(user.Email, ct);
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.UserName, userRoles.data);

            return new UserDto()
            {
                Email = loginDto.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
            };
            //return result + user dto
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            //create application user
          var userresult = await _identityService.CreateUserAsync(registerDto, ct);
            if (!userresult.IsSuccess)
            {
                return Result<UserDto>.Fail(userresult.Errors);
            }
            var user = userresult.data;
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = "Token"
            });
        }
    }
}
