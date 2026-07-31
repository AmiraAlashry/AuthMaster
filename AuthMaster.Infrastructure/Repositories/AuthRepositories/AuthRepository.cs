using AuthMaster.Domain.Entities;
using AuthMaster.Domain.Interfaces.AuthRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Infrastructure.Repositories.AuthRepositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<(bool IsSuccess, string ErrorMessage)> RegisterUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
            return (true,string.Empty);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
            

        }

        public async Task<string> GenerateEmailOtpAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            return token ?? string.Empty;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AddToRoleAsync(ApplicationUser user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return (false, $"Role '{role}' does not exist in the system.");
            }
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
            return (true, string.Empty);
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }   
            return (true, string.Empty);
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> VerifyEmailOtpAsync(ApplicationUser user, string otpCode)
        {
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, otpCode);
            if (!isValid)
            {
                return (false, "Invalid or expired OTP code.");
            }
            return (true, string.Empty);
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
            return (true, string.Empty);
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateSecurityStampAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateSecurityStampAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
            return (true, string.Empty);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsLockedOutAsync(ApplicationUser user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task AccessFailedAsync(ApplicationUser user)
        {
            await _userManager.AccessFailedAsync(user);
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
        }

        public async Task<ApplicationUser?> GetUserWithTokensByEmailAsync(string email)
        {
            return await _userManager.Users
                          .Include(u => u.RefreshTokens)
                          .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
