namespace CarSpot.Application.DTOs;

public record UpdateModelRequest(Guid Id, string Name, Guid MakeId);
