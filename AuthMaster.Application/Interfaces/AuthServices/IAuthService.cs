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
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> VerifyOtpAsync(VerifyOtpDto request);
        Task<AuthResponseDto> ResendOtpAsync(ResendOtpDto request);

    }
}
