using FluentMigrator;

namespace Phoenix.Migrations.Migrations.Colors
{
    [Migration(202301292317)]
    public class _202301292317_AddColorTables : Migration
    {
        public override void Down()
        {
            Delete.Table("Colors");
        }

        public override void Up()
        {
            Create.Table("Colors")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Title").AsString(2000).NotNullable();
        }
    }
}
