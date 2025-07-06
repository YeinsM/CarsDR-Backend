using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
