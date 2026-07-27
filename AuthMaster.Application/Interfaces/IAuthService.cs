using AuthMaster.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> VerifyOtpAsync(VerifyOtp request);
        Task<AuthResponse> ResendOtpAsync(ResendOtp request);

    }
}
