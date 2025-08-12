using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace CarSpot.Application.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                throw new InvalidOperationException("User ID claim not found.");

            return Guid.Parse(userIdClaim);
        }
    }
}
