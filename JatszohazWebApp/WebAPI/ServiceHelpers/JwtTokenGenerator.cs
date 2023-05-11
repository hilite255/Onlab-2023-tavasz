using SharedProject.DbModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.ServiceHelpers
{
    public class JwtTokenGenerator : Domain.Services.ITokenGenerator
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SymmetricSecurityKey Key { get; set; }
        
        public object GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Permission.ToString()),
            };
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Issuer, Audience, claims, null, DateTime.Now.AddDays(1), credentials);
            return new { token = new JwtSecurityTokenHandler().WriteToken(token),
                role = user.Permission.ToString(),
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
            };
        }
    }
}
