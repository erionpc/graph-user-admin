using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Configuration
{
    public class GraphClientConfiguration
    {
        public string? TenantId { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? GraphApiBaseUrl { get; set; }
        public string[]? Scopes { get; set; }
    }
}
