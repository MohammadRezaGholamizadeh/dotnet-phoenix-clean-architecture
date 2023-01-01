using FluentMigrator;

namespace Phoenix.Migrations.Migrations.SerilogTables
{
    [Migration(202212152157)]
    public class _202212152157_AddSeriLogTables : Migration
    {
        public override void Down()
        {
            Delete.Table("ApplicationLogs");
        }

        public override void Up()
        {
            Create.Table("ApplicationLogs")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("LogTimeStamp").AsDateTime().Nullable()
                .WithColumn("RecordNum").AsInt32().Nullable()
                .WithColumn("ComputerName").AsString().Nullable()
                .WithColumn("ProcessTimeStamp").AsDateTime().Nullable()
                .WithColumn("LogGroup").AsString().Nullable()
                .WithColumn("Type").AsString().Nullable()
                .WithColumn("EventId").AsInt32().Nullable()
                .WithColumn("UserId").AsString().Nullable()
                .WithColumn("Line").AsInt32().Nullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Source").AsString().Nullable()
                .WithColumn("Data").AsString().Nullable()
                .WithColumn("AddTimestamp").AsDateTime().Nullable()
                .WithColumn("DeviceID").AsString().Nullable();
        }
    }
}
