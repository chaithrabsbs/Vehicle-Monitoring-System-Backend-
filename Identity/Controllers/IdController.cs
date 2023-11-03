using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public IdController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Token")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Token(UserModel model)
        {
            if (model.LoginID == "admin" && model.Password == "password")
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", model.LoginID)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: signIn);
                model.UserMessage = "Login Success";
                model.UserToken = new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                model.UserMessage = "Login Failed";
            }
            return Ok(model);
        }

    }
}
