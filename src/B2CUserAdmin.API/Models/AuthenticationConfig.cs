using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Models
{
    public class AuthenticationConfig
    {
        public string Instance { get; set; }
        public string TenantName { get; set; }
        public string ClientId { get; set; }
        public string ApplicationIdUri { get; set; }
        public string Authority { get; set; }
        public string AuthorizationUrl { get; set; }
        public string ClientSecret { get; set; }
        public string GraphApiBaseUrl { get; set; }
    }

    public class ApiClientConfig<T>
    {
        public string BasePath { get; set; }
        public IList<string> Scopes { get; set; }
        public string Path { get; set; }
        public AuthType AuthenticationType { get; set; } = AuthType.Delegated;
    }

    public enum AuthType
    {
        Delegated, Application
    }
}
