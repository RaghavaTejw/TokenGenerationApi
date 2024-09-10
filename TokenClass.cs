using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TokenGenerationApi.Models;

namespace TokenGenerationApi
{
    public class TokenClass
    {
        private readonly IConfiguration _configuration;
        private readonly UserContext _userContext;
        public TokenClass(IConfiguration configuration, UserContext userContext)
        {
            _configuration = configuration;
            _userContext = userContext;
        }


        // method to generate the JWT token
        public string GenerateToken(IEnumerable<Claim> claims)
        {


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // method to refresh the token
        //public string GenerateRefreshToken()
        //{
        //    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        //}


        // method to get the details from the token
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidAudience = _configuration["Jwt:Audience"],

                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }


        // method that generates a new access token once the old token is expired
        public string GenerateNewToken(string token)
        {
            var principal = GetPrincipalFromExpiredToken(token);

            var username = principal.Identity?.Name;
            var user = _userContext.LoginModels.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return "Invalid User";
            }

            var expiryClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expiryClaim == null)
            {
                return "Invalid token. Expiry claim missing";
            }

            if (!long.TryParse(expiryClaim.Value, out var expiryUnixTime))
            {
                return "Invalid token: Expiry time format is incorrect.";
            }

            DateTime expiryTime = DateTimeOffset.FromUnixTimeSeconds(expiryUnixTime).UtcDateTime;

            TimeSpan timeRemaining = expiryTime - DateTime.UtcNow;

            if (timeRemaining.TotalSeconds <= 0)
            {
                return "Token Expired. Please login again";
            }

            if (timeRemaining.TotalSeconds < 10)
            {
                var newtoken = GenerateToken(principal.Claims);
                return newtoken;
            }
            return $"Token has {timeRemaining.TotalSeconds} seconds of time. please try again later";


        }
    }
}
