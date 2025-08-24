using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarSpot.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(User user)
    {

        string secret = _configuration["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("JwtSettings:Secret is missing in configuration.");

        string issuer = _configuration["JwtSettings:Issuer"]
            ?? throw new InvalidOperationException("JwtSettings:Issuer is missing in configuration.");

        string audience = _configuration["JwtSettings:Audience"]
            ?? throw new InvalidOperationException("JwtSettings:Audience is missing in configuration.");

        string expiryMinutesValue = _configuration["JwtSettings:ExpiryMinutes"]
            ?? throw new InvalidOperationException("JwtSettings:ExpiryMinutes is missing in configuration.");

        if (!int.TryParse(expiryMinutesValue, out int expiryMinutes))
            throw new InvalidOperationException("JwtSettings:ExpiryMinutes must be a valid integer.");


        DateTime now = DateTime.UtcNow;

       
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role?.Description ?? "User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

     
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = now.AddMinutes(expiryMinutes),
            NotBefore = now,
            IssuedAt = now,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        
        return tokenHandler.WriteToken(token);
    }
}
