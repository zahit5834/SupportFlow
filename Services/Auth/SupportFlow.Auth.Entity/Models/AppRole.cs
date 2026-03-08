using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Auth.Entity.Models
{
    public class AppRole: IdentityRole<Guid>
    {
        // Rollere Özel Açıklama Alanı için kullanılabilir.
        public string? Description { get; set; }
    }
}
