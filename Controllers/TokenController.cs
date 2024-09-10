using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TokenGenerationApi.Models;

namespace TokenGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly TokenClass _tokenClass;

        public TokenController(UserContext usercontext, TokenClass tokenclass)
        {
            _tokenClass = tokenclass;
            _userContext = usercontext;
        }

        [HttpPost]
        [Route("Refresh")]
        public IActionResult RefreshToken(string token)
        {
            var result = _tokenClass.GenerateNewToken(token);

            return Ok(result);
        }

        //[HttpPost]
        //[Route("refresh")]
        //public IActionResult Refresh(TokenData tokendata)
        //{
        //    if (tokendata is null)
        //    {
        //        return BadRequest();
        //    }

        //    var accesstoken = tokendata.AccessToken;
        //    var refreshtoken = tokendata.RefreshToken;

        //    var principal = _tokenClass.GetPrincipalFromExpiredToken(accesstoken);
        //    var username = principal.Identity.Name;

        //    var user = _userContext.LoginModels.FirstOrDefault(u => u.UserName == username);

        //    if (user is null || user.RefreshToken != refreshtoken || user.RefreshTokenExpiryTime < DateTime.Now)
        //        return BadRequest("Please login ");
        //    //var claims = new List<Claim>
        //    //{
        //    //    new Claim(ClaimTypes.Name,user.UserName),
        //    //    new Claim("password",user.Password),
        //    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    //};

        //    var newAccesToken = _tokenClass.GenerateToken(principal.Claims);
        //    var newRefreshToken = _tokenClass.GenerateRefreshToken();

        //    user.Token = newAccesToken;
        //    user.RefreshToken = newRefreshToken;
        //    _userContext.SaveChanges();


        //    return Ok(new
        //    {
        //        AccesToken = newAccesToken,
        //        RefreshToken = newRefreshToken
        //    });
        //}



        //[HttpPost, Authorize]
        //[Route("revoke")]
        //public IActionResult Revoke()
        //{
        //    var username = User.Identity.Name;
        //    var user = _userContext.LoginModels.SingleOrDefault(u => u.UserName == username);
        //    if (user == null) return BadRequest();
        //    user.RefreshToken = null;
        //    _userContext.SaveChanges();
        //    return NoContent();
        //}

    }
}
