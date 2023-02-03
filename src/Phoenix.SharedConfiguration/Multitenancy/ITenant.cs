namespace Phoenix.SharedConfiguration.Multitenancy
{
    public interface ITenant
    {
        public string TenantId { get; set; }
    }
}
