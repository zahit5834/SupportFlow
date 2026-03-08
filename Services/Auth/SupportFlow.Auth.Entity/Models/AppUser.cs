using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Auth.Entity.Models
{
    public class AppUser:IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;

        // Multi-Tenant: Bu kullanıcı hangi şirketin personeli veya müşterisi
        public Guid CompanyId { get; set; }

        // Güvenlik: JWT Refresh Token mekanizması için
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
