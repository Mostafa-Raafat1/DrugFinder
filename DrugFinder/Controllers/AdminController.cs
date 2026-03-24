using Application.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrugFinder.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly IUserService userService;

        public AdminController(IRoleService roleService, IUserService userService)
        {
            this.roleService = roleService;
            this.userService = userService;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await roleService.CreateRoleAsync(roleName);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Role created successfully" });
            }
            else
            {
                return BadRequest(new { Message = result.Error });
            }
        }

        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(string email, string password)
        {
            var result = await userService.CreateUserAsync(email, password);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }
            else
            {
                return Ok(new { Message = "User created successfully" });
            }
        }
        [HttpPost("AssignUserTorole")]
        public async Task<IActionResult> AssignUserToRole(string appUserId, string roleName)
        {
            var result = await userService.AssignUserToRole(appUserId, roleName);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }
            else
            {
                return Ok(new { Message = "User assigned to role successfully" });
            }
        }
    }
}
