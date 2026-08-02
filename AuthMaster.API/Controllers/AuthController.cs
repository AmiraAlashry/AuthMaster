using AuthMaster.Application.DTOs.Auth;
using AuthMaster.Application.DTOs.AuthDTOs;
using AuthMaster.Application.Helpers;
using AuthMaster.Application.Interfaces.AuthServices;
using AuthMaster.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace AuthMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterRequestDto> _registerRequestDtovalidator;
        private readonly IValidator<VerifyOtpDto> _verifyOtpDtovalidator;
        private readonly IValidator<ResendOtpDto> _resendOtpDtovalidator;
        private readonly IValidator<LoginRequestDto> _loginRequestDtovalidator;

        public AuthController(IAuthService authService, IValidator<RegisterRequestDto> registerRequestDtovalidator, IValidator<VerifyOtpDto> verifyOtpDtovalidator, IValidator<ResendOtpDto> resendOtpDtovalidator, IValidator<LoginRequestDto> loginRequestDtovalidator)
        {
            _authService = authService;
            _registerRequestDtovalidator = registerRequestDtovalidator;
            _verifyOtpDtovalidator = verifyOtpDtovalidator;
            _resendOtpDtovalidator = resendOtpDtovalidator;
            _loginRequestDtovalidator = loginRequestDtovalidator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var validationResult = await _registerRequestDtovalidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationFormat.FormatErrors(validationResult));
            }
            var response = await _authService.RegisterAsync(request);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto request)
        {
            var validationResult = await _verifyOtpDtovalidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationFormat.FormatErrors(validationResult));
            }

            var response = await _authService.VerifyOtpAsync(request);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto request)
        {
            var validationResult = await _resendOtpDtovalidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationFormat.FormatErrors(validationResult));
            }
            var response = await _authService.ResendOtpAsync(request);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var validationResult = await _loginRequestDtovalidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationFormat.FormatErrors(validationResult));
            }
            var response = await _authService.LoginAsync(request);
            if (!response.IsSuccess)
            {
                return BadRequest(new
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                });
            }
            SetRefreshTokenInCookie(response.RefreshToken, response.RefreshTokenExpiration);
            return Ok(response);
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expiresOn)
        {
            var cookieExpires = new DateTimeOffset(expiresOn, TimeSpan.FromHours(3));
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpires,
                Secure = true, 
                SameSite = SameSiteMode.None 
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
