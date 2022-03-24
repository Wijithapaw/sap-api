using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAP.Api.Authorization;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using SAP.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<LoginResultDto> Login(LoginRequestDto loginRequest)
        {
            var result = await _userService.LoginAsync(loginRequest);
            return result;
        }

        [HttpGet("ProjectManagers")]
        [ClaimAuthorize(CustomClaims.ProjectsFullAccess)]
        public async Task<List<ListItemDto>> GetProjectManagers()
        {
            return await _userService.GetUsersListItemsByRoleAsync(IdentityRoles.ProjectManager);
        }
    }
}
