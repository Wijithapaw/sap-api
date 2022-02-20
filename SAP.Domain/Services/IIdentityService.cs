using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface IIdentityService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto loginRequest);
    }
}
