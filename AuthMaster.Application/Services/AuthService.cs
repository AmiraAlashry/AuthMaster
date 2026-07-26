using AuthMaster.Application.DTOs.Auth;
using AuthMaster.Application.Helpers;
using AuthMaster.Application.Interfaces;
using AuthMaster.Domain.Entities;
using AuthMaster.Domain.Enums;
using AuthMaster.Domain.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthService> _logger;
        public AuthService(IAuthRepository authRepository, IEmailService emailService, ILogger<AuthService> logger)
        {
            _authRepository=authRepository;
            _emailService = emailService;
            _logger = logger;
        }       
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);

            if (existingUser != null)
            {
                if (string.IsNullOrEmpty(existingUser.PasswordHash))
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "This email is already registered via Google. Please log in using your Google account."
                    };
                }
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "This email is already registered. Please log in."
                };
            }
            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Type = UserType.User,
                UserName = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
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
                var isDeleted = await _authRepository.DeleteUserAsync(newUser);
                if (!isDeleted)
                {
                    _logger.LogCritical("CRITICAL in SIKKA Auth: Failed to delete orphan user with Email {Email} and ID {UserId}. Manual deletion required.", newUser.Email, newUser.Id);
                }
                else
                {
                    _logger.LogWarning("AuthMaster: Successfully rolled back and deleted user {Email} because the role {RoleName} could not be assigned (Ensure it exists in the database).", newUser.Email, newUser.Type.ToString());
                }

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

        public async Task<AuthResponse> VerifyOtpAsync(VerifyOtp request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "This account was registered using Google. OTP verification is not required."
                };
            }
            if (user.EmailConfirmed)
                {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email is already verified. You can log in directly."
                };
            }
            var verifyResult = await _authRepository.VerifyEmailOtpAsync(user, request.OtpCode);
            if (!verifyResult.IsSuccess)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = verifyResult.ErrorMessage
                };
            }
            user.EmailConfirmed = true;
            user.VerifiedAt = DateTime.UtcNow;
            var updateResult = await _authRepository.UpdateUserAsync(user);
            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update user {Email} after OTP success. Errors: {Errors}", request.Email, updateResult.ErrorMessage);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email verification failed due to a system error. Please try again."
                };
            }
            var stampResult = await _authRepository.UpdateSecurityStampAsync(user);
            if (!stampResult.IsSuccess)
            {
                _logger.LogWarning("Email verified for {Email}, but failed to update Security Stamp. Errors: {Errors}", request.Email, stampResult.ErrorMessage);
            }

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Email verification successful. You can now log in."
            };
        }
    }
}
