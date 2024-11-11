using System;
namespace DatabaseMetadataReporting.Options;

public class DatabaseOptions
{
    public const string Key = "DatabaseConnection";
    public string Source { get; set; } = String.Empty;
    public string Database { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}


