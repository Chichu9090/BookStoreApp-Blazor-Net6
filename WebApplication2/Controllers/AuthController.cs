using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Data;
using WebApplication2.Models.User;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<APIUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger,IMapper mapper,UserManager<APIUser> userManager,IConfiguration configuration)

        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Insufficent Data Provided");
            }

            var user = mapper.Map<APIUser>(userDto);
            user.UserName = user.Email;

            var result = await userManager.CreateAsync(user,userDto.Password);

            if(result.Succeeded == false)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await userManager.AddToRoleAsync(user, "User");
            return Accepted();  

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto loginUserDto)
        {
            logger.LogInformation($"Login Attempt for {loginUserDto.Email} ");
            try
            {
                var user = await userManager.FindByEmailAsync(loginUserDto.Email);
                var passwordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);

                if (user == null || !passwordValid)
                {
                    return Unauthorized(loginUserDto);
                }

                string tokenString = await GenerateToken(user);

                var response = new AuthResponse
                {
                    EmailId = loginUserDto.Email,
                    Token = tokenString,
                    UserID = user.Id,
                };

                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something went wrong in the {nameof(Login)} ");
                return Problem($"Something went wrong in the {nameof(Login)} ", statusCode: 500);
            }
        }

        private async Task<string> GenerateToken(APIUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var userClaims = await userManager.GetClaimsAsync(user);


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
                signingCredentials: credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
