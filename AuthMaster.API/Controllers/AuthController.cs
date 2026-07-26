using AuthMaster.Application.DTOs.Auth;
using AuthMaster.Application.Helpers;
using AuthMaster.Application.Interfaces;
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
        private readonly IValidator<RegisterRequest> _validator;
        private readonly IValidator<VerifyOtp> _VerifyOtpvalidator;


        public AuthController(IAuthService authService, IValidator<RegisterRequest> validator, IValidator<VerifyOtp> VerifyOtpvalidator)
        {
            _authService = authService;
            _validator = validator;
            _VerifyOtpvalidator = VerifyOtpvalidator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
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
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtp request)
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
    }
}
