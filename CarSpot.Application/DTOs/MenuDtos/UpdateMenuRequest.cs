using System.ComponentModel.DataAnnotations;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.DTOs
{
   public record UpdateMenuRequest(
        [Required][StringLength(100)] string Label,
        [Required][StringLength(50)] string Icon,
        [Required][StringLength(100)] List<Menu[]> Menub,
        [Required]
        string? To
    );
}