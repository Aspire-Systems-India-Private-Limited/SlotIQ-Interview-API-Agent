using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;

namespace SlotIQ.Interview.Logic.Services;

public interface IJwtTokenService
{
    string GenerateToken(Member member);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        
        // Validate settings
        if (string.IsNullOrEmpty(_jwtSettings.Secret))
            throw new InvalidOperationException("JWT Secret not configured");
        if (string.IsNullOrEmpty(_jwtSettings.Issuer))
            throw new InvalidOperationException("JWT Issuer not configured");
        if (string.IsNullOrEmpty(_jwtSettings.Audience))
            throw new InvalidOperationException("JWT Audience not configured");
        if (_jwtSettings.ExpirationInMinutes <= 0)
            throw new InvalidOperationException("JWT ExpirationInMinutes must be greater than 0");
    }

    public string GenerateToken(Member member)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, member.MemberID.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, member.EmailID),
            new Claim(ClaimTypes.Name, member.UserName),
            new Claim(ClaimTypes.Role, member.RoleID.ToString()),
            new Claim("MemberID", member.MemberID.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
