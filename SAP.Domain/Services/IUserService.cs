using SAP.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Domain.Services
{
    public interface IUserService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto loginRequest);
        Task<List<ListItemDto>> GetUsersListItemsByRoleAsync(string roleName);
        Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordDto dto);
        Task<string> RegisterUserAsync(UserRegisterDto dto);
    }
}
