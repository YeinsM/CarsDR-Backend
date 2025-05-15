using CarSpot.Domain.Common;

public class Currency : BaseEntity
{
    public string Name { get; set; }         
    public string Code { get; set; }          
    public string Symbol { get; set; }     
    public bool IsActive { get; set; }        
}
