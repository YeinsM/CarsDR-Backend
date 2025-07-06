using System.ComponentModel.DataAnnotations.Schema;
using CarSpot.Domain.Common;
using CarSpot.Domain.Events;
using CarSpot.Domain.ValueObjects;

namespace CarSpot.Domain.Entities
{
    public class Business : BaseEntity
    {
        public string? Name { get; set; }
        public string? BusinessNumber { get; set; }
        public string? Phone { get; set; }
        public string? Extension { get; set; }
        public string? Address { get; set; }


    }
}
