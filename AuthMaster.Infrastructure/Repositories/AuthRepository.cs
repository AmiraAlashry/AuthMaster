using AuthMaster.Domain.Entities;
using AuthMaster.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Infrastructure.Repositories
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

        public async Task<string> GenerateEmailOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException($"Unexpected error: User with email '{email}' was not found after creation.");
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            return token ?? string.Empty;
        }

        public async Task<bool> AddToRoleAsync(ApplicationUser user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return false;
            }
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
