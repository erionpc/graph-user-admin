using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2CUserAdmin.UI
{
    public sealed class HttpConfiguration
    {
        public string? Name { get; set; }
        public Uri? BaseAddress { get; set; }
        public string[]? Scopes { get; set; }
    }
}
