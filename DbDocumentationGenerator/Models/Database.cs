using System.Text.Json.Serialization;

namespace DatabaseMetadataReporting.Models;

public class Database
{
    public required string Name { get; set; }
    public string Host { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<Table> Tables { get; set; } = new List<Table>();
    public IEnumerable<string> Modules => Tables.Select(t => t.Module).Distinct().ToList();
    public int TablesCount => Tables.Count();

    public IEnumerable<string> Views { get; set; } = new List<string>();
    public int ViewsCount => Views.Count();

    public IEnumerable<string> Procedures { get; set; } = new List<string>();
    public int ProceduresCount => Procedures.Count();

    public IEnumerable<string> Functions { get; set; } = new List<string>();
    public int FunctionsCount => Functions.Count();
}
