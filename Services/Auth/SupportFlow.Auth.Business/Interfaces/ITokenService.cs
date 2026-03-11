using SupportFlow.Auth.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Auth.Business.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user, IList<string> roles);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
