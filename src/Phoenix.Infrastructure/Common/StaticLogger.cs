using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace Phoenix.Infrastructure.Common;

public static class StaticLogger
{
    private static ILogger _Logger => Log.Logger;

    private static string _connectionStirng
        => ConfigurationTools.GetConnectionString();
    private static string _logTableName
    => ConfigurationTools.GetLogTableName();
    private static ColumnOptions _sqlColumnOptions
    => GetSqlColumnOptions();
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Serilog.Core.Logger)
        {
            Log.Logger =
                new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                         connectionString: _connectionStirng,
                         tableName: _logTableName,
                         columnOptions: _sqlColumnOptions,
                         restrictedToMinimumLevel: LogEventLevel.Debug,
                         batchPostingLimit: 1)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
        }
    }
    public static ColumnOptions GetSqlColumnOptions()
    {
        var colOptions = new ColumnOptions();

        colOptions.Store.Remove(StandardColumn.Properties);
        colOptions.Store.Remove(StandardColumn.MessageTemplate);
        colOptions.Store.Remove(StandardColumn.Message);
        colOptions.Store.Remove(StandardColumn.Exception);
        colOptions.Store.Remove(StandardColumn.TimeStamp);
        colOptions.Store.Remove(StandardColumn.Level);

        colOptions.AdditionalColumns = new Collection<SqlColumn>();

        colOptions.AdditionalColumns
            .AddSqlColumn(columnName: "LogTimeStamp", SqlDbType.DateTime)
            .AddSqlColumn(columnName: "RecordNum", SqlDbType.Int)
            .AddSqlColumn(columnName: "ComputerName", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "ProcessTimeStamp", SqlDbType.DateTime)
            .AddSqlColumn(columnName: "LogGroup", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "Type", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "EventId", SqlDbType.Int)
            .AddSqlColumn(columnName: "UserId", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "Line", SqlDbType.Int)
            .AddSqlColumn(columnName: "Description", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "Source", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "Data", SqlDbType.NVarChar)
            .AddSqlColumn(columnName: "AddTimestamp", SqlDbType.DateTime)
            .AddSqlColumn(columnName: "DeviceID", SqlDbType.NVarChar);

        return colOptions;
    }
    public static void WriteError(ApplicationLog infoToLog)
    {
        try
        {
            _Logger.Error(
                    $"LogTimestamp : {infoToLog.LogTimestamp}" + Environment.NewLine +
                    $"RecordNum : {infoToLog.RecordNum}" + Environment.NewLine +
                    $"ComputerName : {infoToLog.ComputerName}" + Environment.NewLine +
                    $"ProcessTimeStamp : {infoToLog.ProcessTimestamp}" + Environment.NewLine +
                    $"LogGroup : {infoToLog.LogGroup}" + Environment.NewLine +
                    $"Type : {infoToLog.Type}" + Environment.NewLine +
                    $"EventId : {infoToLog.EventId}" + Environment.NewLine +
                    $"UserId : {infoToLog.UserId}" + Environment.NewLine +
                    $"Line : {infoToLog.Line}" + Environment.NewLine +
                    $"Description : {infoToLog.Description}" + Environment.NewLine +
                    $"Source : {infoToLog.Source}" + Environment.NewLine +
                    $"Data : {infoToLog.Data}" + Environment.NewLine +
                    $"AddTimestamp : {infoToLog.AddTimestamp}" + Environment.NewLine +
                    $"DeviceID : {infoToLog.DeviceId}");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
public class ApplicationLog
{
    public DateTime? LogTimestamp { get; set; }
    public int? RecordNum { get; set; }
    public string? ComputerName { get; set; }
    public DateTime? ProcessTimestamp { get; set; }
    public string? LogGroup { get; set; }
    public string? Type { get; set; }
    public int? EventId { get; set; }
    public string? UserId { get; set; }
    public int? Line { get; set; }
    public string? Description { get; set; }
    public string? Source { get; set; }
    public string? Data { get; set; }
    public DateTime? AddTimestamp { get; set; }
    public string? DeviceId { get; set; }
}
