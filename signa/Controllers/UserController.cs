using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto user)
        {
            var userId = await usersService.CreateUser(user);
            return Ok(userId);
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponseDto>> Get(Guid userId)
        {
            var userResponseDto =  await usersService.GetUser(userId);
            return userResponseDto is null ? NotFound() : Ok(userResponseDto);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto user)
        {
            var id = await usersService.UpdateUser(userId, user);
            return Ok(id);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var deletedUserId = await usersService.DeleteUser(userId);
            return Ok(deletedUserId);
        }
    }
}
