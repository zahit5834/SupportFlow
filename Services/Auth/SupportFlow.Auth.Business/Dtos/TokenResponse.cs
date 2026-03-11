using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Auth.Business.Dtos
{
    public record TokenResponseDto(string AccessToken, string RefreshToken, DateTime Expiration);
}
