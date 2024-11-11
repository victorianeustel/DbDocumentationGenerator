using System.Data.SqlClient;

namespace DatabaseMetadataReporting.Models.SQLQueryModels;

public class ColumnQueryResult(SqlDataReader values)
{
    public string DatabaseName { get; set; } = (string)values[0];
    public int TableId { get; set; } = (int)values[1];
    public string TableName { get; set; } = (string)values[2];
    public int ColumnId { get; set; } = (int)values[3];
    public string ColumnName { get; set; } = (string)values[4];
    public string ColumnDataType { get; set; } = (string)values[5];
    public int MaxLength { get; set; } = (short)values[6];
    public int Precision { get; set; } = (byte)values[7];
    public int Scale { get; set; } = (byte)values[8];
    public bool IsNullable { get; set; } = (bool)values[9];
    //public bool IsPrimaryKey { get; set; } = (bool)values[9];
    //public bool IsForeignKey { get; set; } = (bool) values[10];
}
