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
            var keys = await GetPrimaryKeys();

            var model = MappingHelper.ColumnQueryResultToDatabase(options, columns, keys);
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

    public async Task<IEnumerable<PrimaryKeysResult>> GetPrimaryKeys()
    {
        var connectionString = GetSqlConnection();
        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = keyQuery;
            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();
            List<PrimaryKeysResult> keys = new List<PrimaryKeysResult>();
            while (await reader.ReadAsync())
            {
                object[] values = new object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);

                PrimaryKeysResult result = new PrimaryKeysResult(reader);
                keys.Add(result);
            }
            return keys;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private static string query =
        @"
        select
            DB_NAME() AS 'DatabaseName',
            t.object_id TableId,
            t.name 'TableName',
            c.column_id 'ColumnId',
            c.name 'ColumnName'
            , t2.name 'ColumnDataType'
            , t2.max_length as 'MaxLength'
            , t2.PRECISION as 'Precision'
            , t2.scale as 'Scale'
            , c.is_nullable as 'IsNullable'
        from sys.tables t 
        left join sys.columns c on t.object_id = c.object_id
        left join sys.types t2 ON c.system_type_id = t2.system_type_id
        where t2.name != 'sysname' and t.[type] = 'U'
        order by TableName, c.column_id";

    private static string keyQuery = @"
        select
            t.[object_id] as TableId, 
            t.[name] as TableName,
            ic.index_column_id as ColumnId,
            c.[name] as ColumnName, 
            pk.[name] as PkName,
            pk.is_primary_key IsPrimaryKey
        from sys.tables t
            inner join sys.indexes pk
                on t.object_id = pk.object_id 
                and pk.is_primary_key = 1
            inner join sys.index_columns ic
                on ic.object_id = pk.object_id
                and ic.index_id = pk.index_id
            inner join sys.columns c
                on pk.object_id = c.object_id
                and c.column_id = ic.column_id
        ";
}
