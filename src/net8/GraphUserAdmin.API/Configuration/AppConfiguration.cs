namespace GraphUserAdmin.API.Configuration
{
    public class AppConfiguration
    {
        public AuthenticationConfiguration? TenantConfig { get; set; }
        public GraphClientConfiguration? GraphClientConfig { get; set; }
        public CorsConfiguration? CorsConfig { get; set; }
        public int MaxItemsPerPage { get; set; } = 200;
    }
}