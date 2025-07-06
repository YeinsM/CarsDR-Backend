using System.ComponentModel.DataAnnotations;
using CarSpot.Domain.Entities;


namespace CarSpot.Application.DTOs
{
    public record CreateMenuRequest(
       [Required][StringLength(100)] string Label,
       [Required][StringLength(50)] string Icon,
       string? To,
       Guid? ParentId
   );
}
