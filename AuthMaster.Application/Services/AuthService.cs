using AuthMaster.Application.DTOs.Auth;
using AuthMaster.Application.Helpers;
using AuthMaster.Application.Interfaces;
using AuthMaster.Domain.Entities;
using AuthMaster.Domain.Enums;
using AuthMaster.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IEmailService _emailService;
        public AuthService(IAuthRepository authRepository, IEmailService emailService)
        {
            _authRepository=authRepository;
            _emailService = emailService;
        }       
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var isEmailExists = await _authRepository.CheckEmailExistsAsync(request.Email);

            if (isEmailExists)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email already exists."
                };
            }
            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Type = UserType.User,
                UserName = Guid.NewGuid().ToString()

            };
            var result = await _authRepository.RegisterUserAsync(newUser,request.Password);

            if (!result.IsSuccess)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = result.ErrorMessage
                };
            }
            var roleAssigned = await _authRepository.AddToRoleAsync(newUser, newUser.Type.ToString());
            if (!roleAssigned)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Registration failed due to a system error. Please try again."
                };
            }
            var otpCode = await _authRepository.GenerateEmailOtpAsync(newUser.Email);
            if (string.IsNullOrEmpty(otpCode))
            {
                return new AuthResponse
                {
                    IsSuccess = true,
                    Message = "Account created successfully, but we failed to generate the OTP code. Please try requesting a new code."
                };
            }
            var userName = $"{newUser.FirstName} {newUser.LastName}";
            var emailBody = EmailTemplates.GenerateOtpEmail(userName, otpCode);
            
            try
            {
                await _emailService.SendEmailAsync(newUser.Email, "Verify Your Account - AuthMaster", emailBody);
            }
            catch (Exception)
            {
                return new AuthResponse
                {
                    IsSuccess = true,
                    Message = "Account created successfully, but we couldn't send the verification email. Please try requesting a new code."
                };
            }
                
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Registration successful. Please check your email for the OTP code to verify your account."
            };
        }
    }
}
