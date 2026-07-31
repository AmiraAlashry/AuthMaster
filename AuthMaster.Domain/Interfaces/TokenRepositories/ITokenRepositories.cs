using AuthMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Interfaces.AuthServices
{
    public interface ITokenRepositories
    {
        Task<JwtSecurityToken> GenerateJwtTokenAsync(ApplicationUser user);
    }
}
