using FluentMigrator;

namespace Phoenix.Migrations.Migrations.Tenants
{
    [Migration(202302031915)]
    public class _202302031915_AddTenantTables : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_ApplicationUsers_Tenants").OnTable("ApplicationUsers");
            Delete.Column("TenantId");
            Delete.Table("Tenants");
        }

        public override void Up()
        {
            Create.Table("Tenants")
                .WithColumn("Id").AsString(450).PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Mobile_CountryCallingCode").AsString().NotNullable()
                .WithColumn("Mobile_Number").AsString().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();

            Alter.Table("ApplicationUsers")
                .AddColumn("TenantId").AsString(450).NotNullable()
                .ForeignKey("FK_ApplicationUsers_Tenants", "Tenants", "Id")
                .OnDelete(System.Data.Rule.Cascade);
        }
    }
}
