using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class Role : BaseAuxiliar
{

public string? Description { get; set; }
public ICollection<User>? Users { get; set; }
}