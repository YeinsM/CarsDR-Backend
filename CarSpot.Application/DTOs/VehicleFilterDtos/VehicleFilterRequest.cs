public record VehicleFilterRequest
(
    int? VehicleTypeId,
    Guid? MakeId,
    Guid? ModelId,
    int? ConditionId,
    int? VehicleVersionId,
    int? CityId
);
