using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public RolesController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewRole([FromBody] NewRoleDto newRoleDto)
        {
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = newRoleDto.Name,
                    RegTime = DateTime.Now
                };

                if (role is null)
                {
                    return NotFound(new { message = "Nincs találat" });
                }

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Felvéve" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }


        }
    }
}
