namespace Phoenix.Infrastructure.Notifications;

public class SignalRConfigurations
{
    public class BackPlane
    {
        public string? Provider { get; set; }
        public string? StringConnection { get; set; }
    }

    public bool UseBackPlane { get; set; }
}
