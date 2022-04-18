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
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IRequestContext _requestContext;

        public UserService(UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IOptions<JwtSettings> jwtSettings,
            IRequestContext requestContext) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _requestContext = requestContext;
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

        public async Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(_requestContext.UserId);

            var identityResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPwd, dto.NewPwd);

            var result = new ChangePasswordResult
            {
                Succeeded = identityResult.Succeeded,
                ErrorMessage = string.Join(", ", identityResult.Errors.Select(e => e.Description))
            };

            return result;
        }

        public async Task<List<ListItemDto>> GetUsersListItemsByRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            var listItems = users
                .Select(u => new ListItemDto(u.Id, $"{u.FirstName} {u.LastName} | {u.Email}"))
                .ToList();

            return listItems;
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
