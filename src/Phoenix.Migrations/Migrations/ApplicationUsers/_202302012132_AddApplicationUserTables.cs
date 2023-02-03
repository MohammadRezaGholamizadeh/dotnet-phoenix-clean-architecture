using FluentMigrator;

namespace Phoenix.Migrations.Migrations.ApplicationUsers
{
    [Migration(202302012132)]
    public class _202302012132_AddApplicationUserTables : Migration
    {
        public override void Down()
        {
            Delete.Table("ApplicationUserRoles");
            Delete.Table("ApplicationUserLogins");
            Delete.Table("ApplicationUserClaims");
            Delete.Table("ApplicationRoleClaims");
            Delete.Table("ApplicationUserTokens");
            Delete.Table("ApplicationRoles");
            Delete.Table("ApplicationUsers");
        }

        public override void Up()
        {
            Create.Table("ApplicationUsers")
                .WithColumn("Id").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("UserName").AsString(50).NotNullable()
                .WithColumn("NormalizedUserName").AsString(50).NotNullable()
                .WithColumn("PasswordHash").AsString(int.MaxValue).NotNullable()
                .WithColumn("SecurityStamp").AsString(int.MaxValue).Nullable()
                .WithColumn("ConcurrencyStamp").AsString(int.MaxValue).Nullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEnd").AsDateTimeOffset(7).Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsInt32().NotNullable()
                .WithColumn("NationalCode").AsString(10).NotNullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("Mobile_Number").AsString(11).Nullable()
                .WithColumn("Mobile_CountryCallingCode").AsString(5).Nullable()
                .WithColumn("AvatarId").AsString(450).Nullable()
                .WithColumn("AvatarExtension").AsString(10).Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();

            Create.Table("ApplicationRoles")
                .WithColumn("Id").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("NormalizedName").AsString().NotNullable()
                .WithColumn("ConcurrencyStamp").AsString(int.MaxValue).NotNullable();

            Create.Table("ApplicationUserTokens")
                .WithColumn("UserId").AsString(450).PrimaryKey().NotNullable()
                   .ForeignKey(
                       "ApplicationUsers",
                       "Id")
                   .OnDelete(System.Data.Rule.Cascade)
                .WithColumn("LoginProvider").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("Value").AsString(int.MaxValue).Nullable();

            Create.Table("ApplicationRoleClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("RoleId").AsString(450).NotNullable()
                    .ForeignKey("ApplicationRoles", "Id")
                    .OnDelete(System.Data.Rule.Cascade)
                .WithColumn("ClaimType").AsString(int.MaxValue).Nullable()
                .WithColumn("ClaimValue").AsString(int.MaxValue).Nullable();

            Create.Table("ApplicationUserClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserId").AsString(450).NotNullable()
                    .ForeignKey("ApplicationUsers", "Id")
                    .OnDelete(System.Data.Rule.Cascade)
                .WithColumn("ClaimType").AsString(int.MaxValue).Nullable()
                .WithColumn("ClaimValue").AsString(int.MaxValue).Nullable();

            Create.Table("ApplicationUserLogins")
                .WithColumn("LoginProvider").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("ProviderKey").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("ProviderDisplayName").AsString(int.MaxValue).Nullable()
                .WithColumn("UserId").AsString(450).NotNullable()
                    .ForeignKey("ApplicationUsers", "Id")
                    .OnDelete(System.Data.Rule.Cascade);

            Create.Table("ApplicationUserRoles")
                .WithColumn("UserId").AsString(450).PrimaryKey().NotNullable()
                    .ForeignKey("ApplicationUsers", "Id")
                    .OnDelete(System.Data.Rule.Cascade)
                .WithColumn("RoleId").AsString(450).PrimaryKey().NotNullable()
                    .ForeignKey("ApplicationRoles", "Id")
                    .OnDelete(System.Data.Rule.Cascade);
        }
    }
}
