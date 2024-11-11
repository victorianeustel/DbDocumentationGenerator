using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DatabaseMetadataReporting.Models;

public class Table
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Schema { get; set; } = "dbo";
    public string Module => GetModule();
    public IEnumerable<Column> Columns { get; set; } = new List<Column>();

    private string GetModule()
    {
        var regExpression = @"^[A-Za-z]+[^_]";
        var match = Regex.Match(Name, regExpression);
        if (match.Success)
        {
            return match.Groups[0].Value;
        }
        return "dbo";
    }
}
