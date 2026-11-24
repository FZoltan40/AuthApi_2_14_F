using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UsersController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewUser([FromBody] NewUserDto newUserDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = newUserDto.Name,
                Email = newUserDto.Email,
                Password = newUserDto.Password,
                RegTime = DateTime.Now
            };

            if (user is null)
            {
                return NotFound(new { message = "Nincs user" });
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Felvéve" });
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllUser()
        {
            var users = await _context.Users.ToListAsync();

            if (users is null)
            {
                return NotFound(new { message = "Nincs találat" });
            }

            return Ok(users);
        }

        [HttpGet("byId")]
        public async Task<IActionResult> ById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return NotFound(new { message = "Nincs találat" });
            }

            return Ok(new { message = "Lekérdezve" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == updateUserDto.Id);

            if (existingUser is null)
            {
                return NotFound(new { message = "Nincs találat" });
            }

            existingUser.Name = updateUserDto.Name;
            existingUser.Email = updateUserDto.Email;
            existingUser.Password = updateUserDto.Password;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Frissítve" });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                return NotFound(new { message = "Nincs találat" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Törölve" });
        }
    }
}
