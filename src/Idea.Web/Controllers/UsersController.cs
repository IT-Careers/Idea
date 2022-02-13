using Idea.Service;
using Idea.Service.Models;
using Idea.Web.Models.Core;
using Idea.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Idea.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly AppJwtSettings appJwtSettings;

        public UsersController(
            IUserService userService,
            IOptions<AppJwtSettings> appSettings)
        {
            this.userService = userService;
            this.appJwtSettings = appSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsersLoginBindingModel usersLoginBindingModel)
        {
            var user = await this.userService.AuthenticateAsync(usersLoginBindingModel.Username, usersLoginBindingModel.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is invalid..." });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appJwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenString
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersRegisterBindingModel usersRegisterBindingModel)
        {
            return Created("/", await this.userService.CreateAsync(new IdeaUserServiceModel
            {
                Username = usersRegisterBindingModel.Username,
                Password = usersRegisterBindingModel.Password,
            }));
        }
    }
}
