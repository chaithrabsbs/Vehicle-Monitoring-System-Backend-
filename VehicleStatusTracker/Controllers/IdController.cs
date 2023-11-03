
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VehicleStatusTracker.Models;

namespace VehicleStatusTracker.Controllers
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

        [HttpPost("Token")]
        [AllowAnonymous]
        public UserModel Token(UserModel model)
        {
            var response = new UserModel();

            if (IsValidCredentials(model.LoginID, model.Password))
            {
                var token = GenerateToken(model.LoginID);
                response.UserMessage = "Login Success";
                response.UserToken = token;
            }
            else
            {
                response.UserMessage = "Login Failed";
            }

            return response;
        }

        private bool IsValidCredentials(string loginID, string password)
        {
            // Implement your credential validation logic here, e.g., checking against a database.
            return loginID == _configuration["AppSettings:LoginID"] && password == _configuration["AppSettings:Password"];
        }

        private string GenerateToken(string loginID)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim("UserId", loginID)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
