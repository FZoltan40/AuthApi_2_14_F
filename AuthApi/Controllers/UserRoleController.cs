using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("userrole")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UserRoleController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] NewUserRoleDto newUserRoleDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newUserRoleDto.UserId);

                var role = await _context.Roles.FindAsync(newUserRoleDto.RoleId);

                if (user is null || role is null)
                {
                    return NotFound(new { message = "Nincs user vagy role" });
                }

                user.Roles.Add(role);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Sikeres összerendelés" });
            }
            catch (Exception e)
            {
                return Ok(new { message = e.Message });
            }
        }
    }
}
