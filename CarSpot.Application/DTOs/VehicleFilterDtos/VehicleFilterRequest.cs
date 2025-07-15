public class VehicleFilterRequest
{
    public string? VehicleType { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Condition { get; set; }
    public string? Version { get; set; }
    public string? City { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public string SortDir { get; set; } = "asc";
}
