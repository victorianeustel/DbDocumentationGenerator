using DatabaseMetadataReporting.Models;
using DatabaseMetadataReporting.Models.SQLQueryModels;
using DatabaseMetadataReporting.Options;
using Microsoft.Extensions.Options;

namespace DatabaseMetadataReporting.Helpers;

public static class MappingHelper
{
    public static Database ColumnQueryResultToDatabase(
        IOptions<DatabaseOptions> databaseOptions,
        IEnumerable<ColumnQueryResult> queryColumns,
        IEnumerable<PrimaryKeysResult> queryKeys)
    {
        var databaseName = queryColumns.FirstOrDefault()?.DatabaseName;
        var tablesGrouped = queryColumns
            .GroupBy(c => new { c.TableId, c.TableName });         ;

        var tables = new List<Table>();
        foreach (var table in tablesGrouped)
        {
            var tableKeys = queryKeys.Where(k => k.TableId == table.Key.TableId);
            var columns = table.Select(c => new Column()
            {
                Index = c.ColumnId,
                Name = c.ColumnName,
                DataType = c.ColumnDataType,
                LengthSize = c.MaxLength,
                Precision = c.Precision,
                Scale = c.Scale,
                Nullable = c.IsNullable,
                IsPrimaryKey = tableKeys.FirstOrDefault(k => k.ColumnId == c.ColumnId) != null,
            }).ToList();
            var tableResult = new Table()
            {
                Id = table.Key.TableId,
                Name = table.Key.TableName,
                Columns = columns,
            };
            tables.Add(tableResult);
        }

        var databaseResult = new Database() {
            Name = databaseName ?? databaseOptions.Value.Database,
            Host = databaseOptions.Value.Source,
            Tables = tables };

        return databaseResult;
    }
}
