using AuthMaster.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Interfaces.AuthServices
{
    public interface IAuthService
    {
        Task<AuthBaseResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthBaseResponseDto> VerifyOtpAsync(VerifyOtpDto request);
        Task<AuthBaseResponseDto> ResendOtpAsync(ResendOtpDto request);

    }
}
