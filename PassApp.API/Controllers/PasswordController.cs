using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassApp.API.Dtos;
using PassApp.API.Models;
using PassApp.API.Repository;

namespace PassApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PasswordController : Controller
    {
        private readonly IAppRepository _repo;
        private readonly IMapper _mapper;
        public PasswordController(IAppRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpPost("New")]
        public async Task<IActionResult> NewPassword([FromBody] PasswordRegister password)
        {
            if(password == null) return BadRequest("Non optional fields cannot be empty.");

            var user = await _repo.GetUser(password.UserId);

            if(user == null)
                ModelState.AddModelError("UserId", "User does not exists.");
            if(await _repo.PasswordExists(password.Name, password.UserId)) 
                ModelState.AddModelError("Name", "Password name already exists in the database.");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var newPassword = _mapper.Map<Password>(password);

            newPassword.Created = DateTime.Now;
            newPassword.Updated = DateTime.Now; 

            await _repo.AddPassword(newPassword, user.UserKey);

            if (await _repo.SaveAll()) return StatusCode(201);

            return BadRequest();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPasswords(int userId)
        {
            // getting the user id
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            if(await _repo.GetUser(userId) == null) return BadRequest("User does not exists.");

            var passwords = await _repo.GetPasswords(userId);
            
            var passwordsReturn = _mapper.Map<List<PasswordReturn>>(passwords.ToList());

            return Ok(passwordsReturn);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePasword([FromBody] PasswordUpdate password)
        {
            if (password == null) return BadRequest();

            // getting the user id
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != password.UserId) return Unauthorized();

            var newPass = _mapper.Map<Password>(password);
            newPass.Updated = DateTime.Now;

            var ok = await _repo.UpdatePassword(newPass);

            if (!ok) return BadRequest("Password cannot be updated");

            if(await _repo.SaveAll()) return StatusCode(204);

            return BadRequest("Server error");
        }

        [HttpGet("{userid}/{passid}")]
        public async Task<IActionResult> ViewUserPassword(int userId, int passId)
        {
            // getting the user id
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            var passwordsUser = await _repo.GetPasswords(currentUserId);
            if (!passwordsUser.Any(x => x.Id == passId)) return Unauthorized();

            var passwordString = await _repo.ViewPassword(userId, passId);

            if (string.IsNullOrEmpty(passwordString)) return BadRequest("Password don't exists");

            return Ok(new { password = passwordString});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            // getting the user id
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var passwordsUser = await _repo.GetPasswords(currentUserId);

            if (!passwordsUser.Any(x => x.Id == id)) return Unauthorized();

            var ok = await _repo.DeletePassword(id);

            if (!ok) return BadRequest("The password you want to delete does not exists");

            if(await _repo.SaveAll()) return StatusCode(204);

            return BadRequest("Cannot delete password.");
        }
    }

}