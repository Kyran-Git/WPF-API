using API.Mappers;
using API.Interfaces;
using API.Models;
using API.DTO.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
            var userDTO = users.Select(s => s.ToUserDTO());
            return Ok(userDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToUserDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userDTO)
        {
            Users user = new Users()
            {
                UserName = userDTO.UserName,
                Password = userDTO.Password
            };
            await _userRepo.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user.ToUserDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDTO updateDTO)
        {
            var user = await _userRepo.UpdateAsync(id, updateDTO.ToUserFromUpdate());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToUserDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userModel = await _userRepo.DeleteAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
