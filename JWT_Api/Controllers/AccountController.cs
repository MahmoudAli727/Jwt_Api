using JWT_Api.Data.Dtos;
using JWT_Api.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
            public AccountController(UserManager<AppUser> userManager, IConfiguration configuration)
            {
                _userManager = userManager;
                this.configuration = configuration;
            }

            private readonly UserManager<AppUser> _userManager;
            private readonly IConfiguration configuration;

            [HttpPost("Register")]
            public async Task<IActionResult> RegisterNewUser(NewUserDto user)
            {
                if (ModelState.IsValid)
                {
                    AppUser appUser = new()
                    {
                        UserName = user.userName,
                        Email = user.email,
                    };
                    IdentityResult result = await _userManager.CreateAsync(appUser, user.password);
                    if (result.Succeeded)
                    {
                        return Ok("Success");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                return BadRequest(ModelState);
            }

            [HttpPost]
            public async Task<IActionResult> LogIn(LoginDto login)
            {
                if (ModelState.IsValid)
                {
                    AppUser? user = await _userManager.FindByNameAsync(login.userName);
                    if (user != null)
                    {
                        if (await _userManager.CheckPasswordAsync(user, login.password))
                        {
                            var claims = new List<Claim>();
                            //claims.Add(new Claim("name", "value"));
                            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                            var roles = await _userManager.GetRolesAsync(user);
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                            }
                            //signingCredentials
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                            var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                claims: claims,
                                issuer: configuration["JWT:Issuer"],
                                audience: configuration["JWT:Audience"],
                                expires: DateTime.Now.AddHours(1),
                                signingCredentials: sc
                                );
                            var _token = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo,
                            };
                            return Ok(_token);
                        }
                        else
                        {
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Name is invalid");
                    }
                }
                return BadRequest(ModelState);
            }
        }
}
