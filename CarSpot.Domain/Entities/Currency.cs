using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities;

public class Currency : BaseEntity
{
    public string Name { get; set; }         
    public string Code { get; set; }          
    public string Symbol { get; set; }     
  

    public Currency(string name, string code, string symbol)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Currency name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Currency symbol cannot be null or empty.", nameof(symbol));

        Name = name;
        Code = code;
        Symbol = symbol;
    }     
}
