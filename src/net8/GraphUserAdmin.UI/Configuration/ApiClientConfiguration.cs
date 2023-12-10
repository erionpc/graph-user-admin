using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphUserAdmin.UI.Configuration
{
    public sealed class ApiClientConfiguration
    {
        public string? Name { get; set; }
        public Uri? BaseAddress { get; set; }
        public string[]? Scopes { get; set; }
    }
}
