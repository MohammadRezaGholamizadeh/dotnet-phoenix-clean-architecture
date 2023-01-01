using System.Collections.ObjectModel;

namespace Phoenix.SharedConfiguration.Authorization;

public static class PhoenixAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class PhoenixResource
{
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
}

public static class PhoenixPermissions
{
    private static readonly PhoenixPermission[] _all = new PhoenixPermission[]
    {
        new("View Dashboard", PhoenixAction.View, PhoenixResource.Dashboard),
        new("View Hangfire", PhoenixAction.View, PhoenixResource.Hangfire),
        new("View Users", PhoenixAction.View, PhoenixResource.Users),
        new("Search Users", PhoenixAction.Search, PhoenixResource.Users),
        new("Create Users", PhoenixAction.Create, PhoenixResource.Users),
        new("Update Users", PhoenixAction.Update, PhoenixResource.Users),
        new("Delete Users", PhoenixAction.Delete, PhoenixResource.Users),
        new("Export Users", PhoenixAction.Export, PhoenixResource.Users),
        new("View UserRoles", PhoenixAction.View, PhoenixResource.UserRoles),
        new("Update UserRoles", PhoenixAction.Update, PhoenixResource.UserRoles),
        new("View Roles", PhoenixAction.View, PhoenixResource.Roles),
        new("Create Roles", PhoenixAction.Create, PhoenixResource.Roles),
        new("Update Roles", PhoenixAction.Update, PhoenixResource.Roles),
        new("Delete Roles", PhoenixAction.Delete, PhoenixResource.Roles),
        new("View RoleClaims", PhoenixAction.View, PhoenixResource.RoleClaims),
        new("Update RoleClaims", PhoenixAction.Update, PhoenixResource.RoleClaims)
    };

    public static IReadOnlyList<PhoenixPermission> All { get; }
            = new ReadOnlyCollection<PhoenixPermission>(_all);

    public static IReadOnlyList<PhoenixPermission> Root { get; }
            = new ReadOnlyCollection<PhoenixPermission>(
                  _all.Where(p => p.isRoot).ToArray());

    public static IReadOnlyList<PhoenixPermission> Admin { get; }
            = new ReadOnlyCollection<PhoenixPermission>(
                  _all.Where(p => !p.isRoot).ToArray());

    public static IReadOnlyList<PhoenixPermission> Basic { get; }
            = new ReadOnlyCollection<PhoenixPermission>(
                  _all.Where(p => p.isBasic).ToArray());
}

public record PhoenixPermission(
    string description,
    string action,
    string resource,
    bool isRoot = false,
    bool isBasic = false)
{
    public string Name => $"Permissions.{resource}.{action}";
}
