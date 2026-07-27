using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Interfaces.CommonServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

    }
}
