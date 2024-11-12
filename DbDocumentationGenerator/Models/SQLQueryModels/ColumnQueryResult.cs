using System.Data.SqlClient;
using DatabaseMetadataReporting.Helpers;

namespace DatabaseMetadataReporting.Models.SQLQueryModels;

public class ColumnQueryResult(SqlDataReader values)
{
    public string DatabaseName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[0]);
    public int TableId { get; set; } = DbValueHelper.ConvertFromDBVal<int>(values[1]);
    public string TableName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[2]);
    public int ColumnId { get; set; } = DbValueHelper.ConvertFromDBVal<int>(values[3]);
    public string ColumnName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[4]);
    public string ColumnDataType { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[5]);
    public int MaxLength { get; set; } = DbValueHelper.ConvertFromDBVal<short>(values[6]);
    public int Precision { get; set; } = DbValueHelper.ConvertFromDBVal<byte>(values[7]);
    public int Scale { get; set; } = DbValueHelper.ConvertFromDBVal<byte>(values[8]);
    public bool IsNullable { get; set; } = DbValueHelper.ConvertFromDBVal<bool>(values[9]);
    public bool IsPrimaryKey { get; set; } = DbValueHelper.ConvertFromDBVal<bool>(values[10]);
    public string? PkName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[11]);
}

