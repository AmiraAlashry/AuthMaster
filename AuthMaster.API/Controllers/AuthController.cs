using AuthMaster.Application.DTOs.Auth;
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


        public AuthController(IAuthService authService, IValidator<RegisterRequestDto> registerRequestDtovalidator, IValidator<VerifyOtpDto> verifyOtpDtovalidator, IValidator<ResendOtpDto> resendOtpDtovalidator)
        {
            _authService = authService;
            _registerRequestDtovalidator = registerRequestDtovalidator;
            _verifyOtpDtovalidator = verifyOtpDtovalidator;
            _resendOtpDtovalidator = resendOtpDtovalidator;
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
    }
}
