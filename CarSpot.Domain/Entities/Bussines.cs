using CarSpot.Domain.Common;
using CarSpot.Domain.ValueObjects;
using CarSpot.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSpot.Domain.Entities
{
    public class Bussines : BaseEntity
    {
        public Guid BussinesNumber {get; set;}
        public string? PhoneBussines {get; set;}
        public string? ExtencionBussines {get; set;}
        public string? AddreesBussines {get; set;}

       

       


    }
}