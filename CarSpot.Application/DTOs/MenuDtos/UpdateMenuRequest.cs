using System.ComponentModel.DataAnnotations;

namespace CarSpot.Application.DTOs
{
    public record UpdateMenuRequest(
        [Required][StringLength(100)] string Label,
        [Required][StringLength(50)] string Icon,
        string? To,
        Guid? ParentId
    );
}
