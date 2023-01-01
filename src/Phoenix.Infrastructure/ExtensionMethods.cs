using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace Phoenix.Infrastructure
{
    public static class ExtensionMethods
    {
        public static ICollection<SqlColumn>
            AddSqlColumn(
                this ICollection<SqlColumn> entity,
                string columnName,
                SqlDbType dataType,
                bool allowNulls = true)
        {
            entity.Add(new SqlColumn(columnName, dataType, allowNulls));
            return entity;
        }
    }
}
