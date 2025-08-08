using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Ambev.DeveloperEvaluation.Domain.Common.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Security
{
    /// <summary>
    /// Implementation of JWT (JSON Web Token) generator.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the JWT token generator.
        /// </summary>
        /// <param name="configuration">Application configuration containing the necessary keys for token generation.</param>
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token for a specific user.
        /// </summary>
        /// <param name="user">User for whom the token will be generated.</param>
        /// <returns>Valid JWT token as string.</returns>
        /// <remarks>
        /// The generated token includes the following claims:
        /// - NameIdentifier (User ID)
        /// - Name (Username)
        /// - Role (User role)
        /// 
        /// The token is valid for 8 hours from the moment of generation.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when user or secret key is not provided.</exception>
        public string GenerateToken(IUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var lifetimeHours = _configuration.GetValue<int?>("Jwt:LifetimeHours") ?? 8;

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Name, user.Username),
               new Claim(ClaimTypes.Role, user.Role.ToString())
           };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(lifetimeHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}