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
        private readonly IValidator<RegisterRequestDto> _validator;
        private readonly IValidator<VerifyOtpDto> _VerifyOtpvalidator;
        private readonly IValidator<ResendOtpDto> _ResendOtpvalidator;


        public AuthController(IAuthService authService, IValidator<RegisterRequestDto> validator, IValidator<VerifyOtpDto> VerifyOtpvalidator, IValidator<ResendOtpDto> resendOtpvalidator)
        {
            _authService = authService;
            _validator = validator;
            _VerifyOtpvalidator = VerifyOtpvalidator;
            _ResendOtpvalidator = resendOtpvalidator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
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
            var validationResult = await _VerifyOtpvalidator.ValidateAsync(request);
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
            var validationResult = await _ResendOtpvalidator.ValidateAsync(request);
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
