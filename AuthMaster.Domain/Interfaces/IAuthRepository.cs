using AuthMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<(bool IsSuccess, string ErrorMessage)> RegisterUserAsync(ApplicationUser user, string password);
        Task<string> GenerateEmailOtpAsync(string email);
        Task<bool> AddToRoleAsync(ApplicationUser user, string role);
        Task<bool> DeleteUserAsync(ApplicationUser user);
        Task<(bool IsSuccess, string ErrorMessage)> VerifyEmailOtpAsync(ApplicationUser user, string otpCode);
        Task<(bool IsSuccess, string ErrorMessage)> UpdateUserAsync(ApplicationUser user);
        Task<(bool IsSuccess, string ErrorMessage)> UpdateSecurityStampAsync(ApplicationUser user);

    }
}
