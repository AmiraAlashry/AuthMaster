using AuthMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Domain.Interfaces.AuthRepositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<(bool IsSuccess, string ErrorMessage)> RegisterUserAsync(ApplicationUser user, string password);
        Task<string> GenerateEmailOtpAsync(ApplicationUser user);
        Task<(bool IsSuccess, string ErrorMessage)> AddToRoleAsync(ApplicationUser user, string role);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteUserAsync(ApplicationUser user);
        Task<(bool IsSuccess, string ErrorMessage)> VerifyEmailOtpAsync(ApplicationUser user, string otpCode);
        Task<(bool IsSuccess, string ErrorMessage)> UpdateUserAsync(ApplicationUser user);
        Task<(bool IsSuccess, string ErrorMessage)> UpdateSecurityStampAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<bool> IsLockedOutAsync(ApplicationUser user);
        Task AccessFailedAsync(ApplicationUser user);
        Task ResetAccessFailedCountAsync(ApplicationUser user);
        Task<ApplicationUser?> GetUserWithTokensByEmailAsync(string email);
    }
}
