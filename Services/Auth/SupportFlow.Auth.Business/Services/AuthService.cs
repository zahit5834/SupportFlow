using Microsoft.AspNetCore.Identity;
using SupportFlow.Auth.Business.Dtos;
using SupportFlow.Auth.Business.Interfaces;
using SupportFlow.Auth.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Auth.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new Exception("Kullanıcı Adı veya Şifre Hatalı!");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _tokenService.CreateAccessToken(user, roles);
            var refreshToken = _tokenService.CreateRefreshToken();

            // Refresh token'ı kullanıcı satırında güncelle

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new TokenResponseDto(accessToken, refreshToken, DateTime.UtcNow.AddMinutes(15));
        }

        public Task<TokenResponseDto> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                CompanyId = registerDto.CompanyId
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(",", result.Errors.Select(x => x.Description)));
            }

            return await LoginAsync(new LoginDto(registerDto.Email, registerDto.Password));

        }
    }
}
