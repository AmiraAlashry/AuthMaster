using AuthMaster.Application.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Validators.AuthValidators
{
    public class ResendOtpDtoValidator : AbstractValidator<ResendOtpDto>
    {
        public ResendOtpDtoValidator() 
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
    
        }
    }
}
