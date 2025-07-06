using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities;

public class Currency
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;

    public Currency() { }
    public Currency(Guid id, string name, string code, string symbol)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Currency name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Currency symbol cannot be null or empty.", nameof(symbol));
        Id = id;
        Name = name;
        Code = code;
        Symbol = symbol;
    }
}
