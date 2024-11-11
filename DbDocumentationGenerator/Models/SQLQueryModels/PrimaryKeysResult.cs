using System.Data.SqlClient;

namespace DatabaseMetadataReporting.Models.SQLQueryModels;

public class PrimaryKeysResult(SqlDataReader values)
{
    public int TableId { get; set; } = (int)values[0];
    public string TableName { get; set; } = (string)values[1];
    public int ColumnId { get; set; } = (int)values[2];
    public string ColumnName { get; set; } = (string)values[3];
    public string PkName { get; set; } = (string)values[4];
    public bool IsPrimaryKey { get; set; } = (bool)values[5];
}
