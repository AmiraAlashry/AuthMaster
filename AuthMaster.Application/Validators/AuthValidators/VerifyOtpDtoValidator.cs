using AuthMaster.Application.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Validators.AuthValidators
{
    public class VerifyOtpDtoValidator : AbstractValidator<VerifyOtpDto>
    {
        public VerifyOtpDtoValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.OtpCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("OTP code is required.")
            .Length(6).WithMessage("OTP code must be exactly 6 characters.")
            .Matches("^[0-9]+$").WithMessage("OTP code must contain numbers only."); ;
        }
    }
}
