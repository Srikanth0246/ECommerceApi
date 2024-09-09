using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AspCoreJwtDb.Models;
 
namespace AspCoreJwtDb.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static List<Account> accounts = new List<Account>
        {
            new Account{ Id = 1, UserName = "admin", Password = "Passcode1" },
            new Account { Id = 2, UserName = "user", Password = "Passcode2" }
        };
 
        private readonly IConfiguration _configuration;
 
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
 
        public IActionResult Post([FromBody] Account account)
        {
            var user = accounts.FirstOrDefault(u => u.UserName == account.UserName && u.Password == account.Password);
 
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(new { token });
            }
 
            return Unauthorized("Invalid Username or Password");
        }
 
        private string GenerateToken(Account account)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ServerSecret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, account.UserName == "admin" ? "Admin" : "Employee")
            };
 
            var now = DateTime.UtcNow;
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: now.Add(TimeSpan.FromHours(1)),
                signingCredentials: signingCredentials
            );
 
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(tokenDescriptor);
        }
    }
}