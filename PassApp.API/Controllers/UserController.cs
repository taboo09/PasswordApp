using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassApp.API.Dtos;
using PassApp.API.Repository;

namespace PassApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IAppRepository _repo;
        private readonly IMapper _mapper;
        public UserController(IAppRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id) 
        {
            var user = await _repo.GetUser(id);

            if (user == null) return NotFound($"User with id: {id} doesn't exist.");
            var passwords = await _repo.GetPasswords(id);

            var userInfo = _mapper.Map<UserInfoDto>(user);
            userInfo.PasswordsCount = passwords.ToList().Count;

            return Ok(userInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] UserLoginDto user)
        {
            bool ok = await _repo.DeleteUser(id, user.Password);

            if (!ok) return BadRequest("Password doesn't match");

            if (await _repo.SaveAll()) return Ok();

            return BadRequest("Cannot delete user."); 
        }

    }
}