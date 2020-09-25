using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;

namespace WebApiJwt.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _app;

        public AccountController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext app,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            _app = app;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult Trial()
        {
            return Ok(_app.Trials.ToArray());
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                string token = GenerateJwtToken(appUser, _configuration);
                return Ok(new { token });
            }
            else
            {
                return StatusCode(500, "Incorrect username or password");
            }
           
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                GenerateJwtToken(user, _configuration);
                return Ok();
            }
            else
            {
                var errorsObj = result.Errors.Select(x => x.Description);
                return StatusCode(500, errorsObj);
            }
            
            //throw new ApplicationException("Something bad is not working");
        }

        private static string GenerateJwtToken(IdentityUser user, IConfiguration _configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_Info:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT_Info:JwtIssuer"],
                audience: _configuration["JWT_Info:JwtIssuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }

        public class RegisterDto
        {
            //[Required]
            public string Email { get; set; }

            //[Required]
            //[StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }
    }
}