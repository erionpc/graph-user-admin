using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Configuration
{
    public class AuthenticationConfiguration
    {
        public string? TenantName { get; set; }
        public string? ClientId { get; set; }
        public string? Audience { get; set; }
        public string? Authority { get; set; }
        public string? AuthorizationUrl { get; set; }
        public string? ClientSecret { get; set; }
    }
}
