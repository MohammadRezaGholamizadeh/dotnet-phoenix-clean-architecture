namespace Phoenix.Infrastructure.ApiVersionings
{
    public class ApiVersioning
    {
        public int MajorApiVersion { get; set; }
        public int MinorApiVersion { get; set; }
        public bool AssumeDefaultVersionWhenUnspecified { get; set; }
        public bool ReportApiVersions { get; set; }
    }
}
