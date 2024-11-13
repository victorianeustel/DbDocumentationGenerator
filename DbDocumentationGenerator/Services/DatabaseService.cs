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
            var views = await GetViews();
            var model = MappingHelper.ColumnQueryResultToDatabase(options, columns, views);
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

            string script = File.ReadAllText(@"Services/Scripts/TablesColumnsQuery.sql");

            await using var command = new SqlCommand(script, connection);
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

    public async Task<IEnumerable<ViewResult>> GetViews()
    {
        var connectionString = GetSqlConnection();
        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string script = File.ReadAllText(@"Services/Scripts/ViewsQuery.sql");

            await using var command = new SqlCommand(script, connection);
            await using var reader = await command.ExecuteReaderAsync();
            List<ViewResult> views = new List<ViewResult>();
            while (await reader.ReadAsync())
            {
                object[] values = new object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);

                ViewResult result = new ViewResult(reader);
                views.Add(result);
            }
            return views;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
