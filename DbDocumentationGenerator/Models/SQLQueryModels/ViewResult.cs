using System;
using System.Data.SqlClient;
using DatabaseMetadataReporting.Helpers;

namespace DatabaseMetadataReporting.Models.SQLQueryModels;

public class ViewResult(SqlDataReader values)
{
    public string DatabaseName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[0]);
    public int SchemaId { get; set; } = DbValueHelper.ConvertFromDBVal<int>(values[1]);
    public string SchemaName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[2]);
    public int ViewId { get; set; } = DbValueHelper.ConvertFromDBVal<int>(values[3]);
    public string ViewName { get; set; } = DbValueHelper.ConvertFromDBVal<string>(values[4]);
}
