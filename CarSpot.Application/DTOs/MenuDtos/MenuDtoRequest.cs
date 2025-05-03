using System.ComponentModel.DataAnnotations;

namespace CarSpot.Application.DTOs
{
    
     public record MenuResponse(
        int Id,
        string Label,
        string Icon,
        string? To,
        int? ParentId,
        List<MenuResponse> Children
    );
}