using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PassApp.API.Dtos;
using PassApp.API.Models;
using PassApp.API.Repository;

namespace PassApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegister)
        {
            if (!string.IsNullOrEmpty(userRegister.UserName))
                userRegister.UserName = userRegister.UserName.ToLower();

            if (await _repo.UserExists(userRegister.UserName))
                ModelState.AddModelError("Username", "username already exists in the database.");

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            
            var user = new User 
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                DateCreated = DateTime.Now
            }; 

            var createdUser = await _repo.Register(user, userRegister.Password);

            return StatusCode(201);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLogin)
        {
            var user = await _repo.Login(userLogin.UserName.ToLower(), userLogin.Password);

            if (user == null) return BadRequest("Username or Entry Code don't match.");

            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.Now.AddMinutes(29),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenString = tokenHandler.WriteToken(token);

            var userReturn = _mapper.Map<UserInfoDto>(user);

            return Ok( new {tokenString, userReturn} );
        }

        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangeEntryCode([FromBody] UserChangePassword user)
        {
            var userCheck = await _repo.Login(user.Username, user.Password);
            if (userCheck == null) return BadRequest("Your password is not vadlid");

            var ok = await _repo.ChangeEntryCode(user.Username, user.Password ,user.NewPassword);

            if (!ok) return BadRequest("Failed to change your password");

            return Ok();
        }
    }
}