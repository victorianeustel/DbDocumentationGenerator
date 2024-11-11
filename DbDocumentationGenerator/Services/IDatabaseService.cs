using DatabaseMetadataReporting.Models;
using DatabaseMetadataReporting.Models.SQLQueryModels;
using System.Data.SqlClient;

namespace DatabaseMetadataReporting.Services;

public interface IDatabaseService
{
    public Task<Database> GetDatabaseMetadata();
    public Task<IEnumerable<ColumnQueryResult>> GetColumns();
    public Task<IEnumerable<PrimaryKeysResult>> GetPrimaryKeys();
}