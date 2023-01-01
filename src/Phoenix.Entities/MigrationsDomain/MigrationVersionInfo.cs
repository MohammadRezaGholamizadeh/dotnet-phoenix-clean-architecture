namespace Phoenix.Domain.MigrationsDomain
{
    public class MigrationVersionInfo
    {
        public long Version { get; set; }
        public DateTime AppliedOn { get; set; }
        public string Description { get; set; }
    }
}
