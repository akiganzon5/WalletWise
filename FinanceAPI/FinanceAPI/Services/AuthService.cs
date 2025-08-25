using FinanceAPI.DTOs;
using FinanceAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceAPI.Services
{
    public class AuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email)
                        ?? await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return null;

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPassword) return null;

            return GenerateJwtToken(user);
        }

        private AuthResponseDto GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["AppSettings:Issuer"],
                audience: _config["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

    }
}
