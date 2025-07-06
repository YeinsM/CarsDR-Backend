using System.ComponentModel.DataAnnotations;

namespace CarSpot.Application.DTOs
{

    public record MenuResponse(
       Guid Id,
       string Label,
       string Icon,
       string? To,
       Guid? ParentId,
       List<MenuResponse> Children
   );
}
