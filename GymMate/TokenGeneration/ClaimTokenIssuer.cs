using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymMate.TokenGeneration
{
    public class ClaimTokenIssuer
    {
        private readonly IConfiguration Config;

        public ClaimTokenIssuer(IConfiguration config)
        {
            Config = config;
        }

        public string GenerateJwtToken()
        {
            var jwtSection = Config.GetSection("Authentication:Schemes:Bearer");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "tarnasi"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Add other claims as needed
            };

            // Add each audience as a separate claim
            foreach (var audience in jwtSection.GetSection("ValidAudiences").Get<List<string>>())
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["SecretKey"])); // Replace with your key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSection["ValidIssuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
