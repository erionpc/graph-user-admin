namespace GraphUserAdmin.UI.Configuration
{
    public class AppConfiguration
    {
        public ApiClientConfiguration? ApiClientConfig { get; set; }
        public TenantConfiguration? TenantConfig { get; set; }
        public int ItemsPerPage { get; set; } = 50;
    }
}
