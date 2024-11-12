using DatabaseMetadataReporting.Helpers;
using DatabaseMetadataReporting.Models;
using DatabaseMetadataReporting.Models.SQLQueryModels;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using DatabaseMetadataReporting.Options;

namespace DatabaseMetadataReporting.Services;

public class DatabaseService(
    ILogger<DatabaseService> logger,
    IOptions<DatabaseOptions> options) : IDatabaseService
{
    private DatabaseOptions databaseOptions = options.Value;
    public async Task<Database> GetDatabaseMetadata()
    {
        try
        {
            var columns = await GetColumns();

            var model = MappingHelper.ColumnQueryResultToDatabase(options, columns);
            return model;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private string GetSqlConnection()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = databaseOptions.Source,
            UserID = databaseOptions.Username,
            Password = databaseOptions.Password,
            InitialCatalog = databaseOptions.Database
        };

        var connectionString = builder.ConnectionString;
        return connectionString;
    }

    public async Task<IEnumerable<ColumnQueryResult>> GetColumns()
    {
        var connectionString = GetSqlConnection();
        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var sql = query;
            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();
            List<ColumnQueryResult> columns = new List<ColumnQueryResult>();
            while (await reader.ReadAsync())
            {
                object[] values = new object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);

                ColumnQueryResult result = new ColumnQueryResult(reader);
                columns.Add(result);
            }
            return columns;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private static string query = @"
        SELECT DB_NAME() [DatabaseName],
            t.object_id [TableId],
            t.name [TableName],
            c.column_id [ColumnId],
            c.name [ColumnName]
                , t2.name [ColumnDataType]
                , t2.max_length [MaxLength]
                , t2.PRECISION [Precision]
                , t2.scale [Scale]
                , c.is_nullable [IsNullable],
            pk.is_primary_key [IsPrimaryKey],
            IIF (pk.[is_primary_key] = 1, pk.[name], NULL) [PkName]
        FROM sys.tables t
            JOIN sys.indexes pk
            ON t.object_id = pk.object_id
            JOIN sys.index_columns ic
            ON ic.object_id = pk.object_id
                AND ic.index_id = pk.index_id
            JOIN sys.columns c
            ON pk.object_id = c.object_id
                AND c.column_id = ic.column_id
            JOIN sys.types t2 ON c.system_type_id = t2.system_type_id
        WHERE t2.name != 'sysname' AND t.[type] = 'U'";
}
