using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.DTOs.Auth
{
    public class VerifyOtp
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;

    }
}
