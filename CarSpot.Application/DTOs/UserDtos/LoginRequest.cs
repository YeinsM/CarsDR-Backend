namespace CarSpot.Application.DTOs;
public record LoginRequest
{
    public string? EmailOrUsername { get; init; }
    public string? Password { get; init; }
}

