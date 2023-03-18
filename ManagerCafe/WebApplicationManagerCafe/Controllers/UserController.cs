using AutoMapper;
using ManagerCafe.Contracts.Dtos.UsersDtos;
using ManagerCafe.Contracts.Services;
using ManagerCafeAPI.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManagerCafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AuthenticationOption _options;
        public UserController(IUserService userService, IMapper mapper, IOptions<AuthenticationOption> options)
        {
            _userService = userService;
            _mapper = mapper;
            _options = options.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetUser(LoginUser loginUser)
        {
            try
            {
                var user = await _userService.Validate(loginUser);
                if (user is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        IsSuccess = false,
                        Message = "Not found user"
                    });
                }
                return StatusCode(StatusCodes.Status200OK, new
                {
                    IsSuccess = true,
                    Message = "Authendicate success",
                    Data = GenerateToken(user)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Serve error " + ex.Message
                });
            }
        }

        private string GenerateToken(UserDto userDto)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,userDto.FullName),
                    new Claim(ClaimTypes.Email,userDto.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
