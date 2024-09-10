using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenGenerationApi.Models;

namespace TokenGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenClass tokenClass;
        private readonly UserContext _userContext;
        public AuthController(TokenClass tokenClass, UserContext userContext)
        {
            this.tokenClass = tokenClass;
            _userContext = userContext;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult AuthenticateUser([FromHeader(Name = "username")] string username, [FromHeader(Name = "password")] string password)
        {

            if (username is null || password is null)
            {
                return BadRequest("Invalid Client request");
            }

            var user = _userContext.LoginModels?.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user is null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,username),
                new Claim("password",password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var accessToken = tokenClass.GenerateToken(claims);

            //var refreshToken = tokenClass.GenerateRefreshToken();

            //user.Token = accessToken;
            //user.RefreshToken = refreshToken;
            //user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            //_userContext.SaveChanges();

            return Ok(new { AccessToken = accessToken });

        }
    }
}
