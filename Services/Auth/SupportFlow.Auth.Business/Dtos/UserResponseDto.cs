using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Auth.Business.Dtos
{
    public record UserResponseDto(Guid Id, string FullName, string Email);
}
