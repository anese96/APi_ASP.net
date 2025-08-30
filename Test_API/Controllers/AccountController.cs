using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test_API.Data.Models;
using Test_API.Models;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<AppUser> userManager ,IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;  
        }

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser(dtoNewUser newUser)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,

                };
                IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    return Ok("Success");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(dtoLogin login)
        {
            if (ModelState.IsValid) { 
            AppUser? User = await _userManager.FindByNameAsync(login.UserName);
                if (User != null) {
                    if (await _userManager.CheckPasswordAsync(User, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, User.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(User);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key=new SymmetricSecurityKey( Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: creds
                            );

                    }
                    else
                    {
                        return Unauthorized(); }

                    } else {
                   ModelState.AddModelError("", "Invalid UserName or Password");
                }
            }

            return BadRequest(ModelState);
        }
    }
}
