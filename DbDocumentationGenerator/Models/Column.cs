using System.Text.Json.Serialization;

namespace DatabaseMetadataReporting.Models;

public class Column
{
    public int Index { get; set; }
    public required string Name { get; set; }
    public required string DataType { get; set; }
    public int LengthSize { get; set; }
    public int Precision { get; set; }
    public int Scale { get; set; }
    public bool Nullable { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPrimaryKey { get; set; }

    public string[] GetPropertyNames() => [
            "", // Index
            "Key",
            nameof(Name),
            "Data Type",
            "Max Length",
            nameof(Precision),
            nameof(Scale),
            nameof(Nullable),
            nameof(Reference),
            nameof(Description)
        ];
}
