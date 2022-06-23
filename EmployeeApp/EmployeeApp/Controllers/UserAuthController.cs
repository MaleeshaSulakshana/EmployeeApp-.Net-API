using EmployeeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {

        private readonly UserDetailsContext _context;
        private readonly IConfiguration _configuration;

        public UserAuthController(UserDetailsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Route for login
        [HttpPost("/api/login")]
        public ActionResult<Object> UserLogin(LoginModel loginModel)
        {
            var userDetails = _context.UserDetails.SingleOrDefault(user => user.Email == loginModel.email);

            // Check email is exist
            if (userDetails == null)
            {
                return BadRequest("Please check your email");
            }

            // Match password
            if (!BCrypt.Net.BCrypt.Verify(loginModel.password, userDetails.Password))
            {
                return BadRequest("Check your password");
            }

            // Create JWT token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });

        }

    }
}
