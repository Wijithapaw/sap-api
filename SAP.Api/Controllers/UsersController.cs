using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<LoginResultDto> Login(LoginRequestDto loginRequest)
        {
            var result = await _identityService.LoginAsync(loginRequest);
            return result;
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            return id;
        }
    }
}
