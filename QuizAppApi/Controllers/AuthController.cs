﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizAppApi.Data;
using QuizAppApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizAppApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(IUserService userService, IPasswordHasher<User> passwordHasher,IConfiguration config)
        {
           _userService = userService;
           _passwordHasher = passwordHasher;
            _config = config;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserAsync(login.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, login.Password) != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(user);
            if (!result)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            return Ok(new { message = "User registered successfully" });
        }

        private string GenerateJwtToken(User user)
        { // iuser pred f language, user roles
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Role, user.Role),
                            new Claim("language_Preference",user.LanguagePreference),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email)
                         };
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

