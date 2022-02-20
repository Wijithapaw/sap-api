using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SAP.Domain.ConfigSettings;
using SAP.Domain.Dtos;
using SAP.Domain.Entities;
using SAP.Domain.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IOptions<JwtSettings> jwtSettings) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);

            if (user != null)
            {
                var succeeded = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (succeeded)
                {
                    var authJwt = await CreateJwtTokenAsync(user);
                    return new LoginResultDto
                    {
                        Succeeded = true,
                        AuthToken = authJwt,
                        Email = user.Email,
                        UserId = user.Id
                    };
                }              
            }

            return new LoginResultDto
            {
                Succeeded = false,
                ErrorCode = "ERR_INVALID_LOGIN_ATTEMPT"
            };
        }

        private async Task<string> CreateJwtTokenAsync(User user)
        {
            var roleNames = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(2);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            var tokenStr = (new JwtSecurityTokenHandler()).WriteToken(token);

            return tokenStr;
        }
    }
}
