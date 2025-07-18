public record VehicleFilterRequest
{
    public Guid? MakeId { get; init; }
    public Guid? ModelId { get; init; }

    public int? MinYear { get; init; } 
    public int? MaxYear { get; init; }  

    public int? ConditionId { get; init; }
    public int? DrivetrainId { get; init; }
    public int? CylinderOptionId { get; init; }
    public int? CabTypeId { get; init; }

    public int? MinMileage { get; init; }
    public int? MaxMileage { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
