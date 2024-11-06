using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.user;
using signa.Interfaces.Services;

namespace signa.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService, IUnitOfWork unitOfWork)
        {
            this.usersService = usersService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponseDto>> Get(Guid userId)
        {
            var userResponseDto =  await usersService.GetUserResponse(userId);
            return userResponseDto != null ? Ok(userResponseDto) : NotFound();
        }
        

        [Authorize(Roles = "Admin,User,Organizer")]
        [HttpGet("search")]
        public async Task<ActionResult<List<UserSearchItemDto>>> GetAllUsersByPrefix([FromQuery] string prefix)
        {
            var foundUsers = await usersService.GetUsersByPrefix(prefix);
            return Ok(foundUsers);
        }

        [Authorize(Roles = "Admin,User,Organizer")]
        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto user)
        {
            var id = await usersService.UpdateUser(userId, user);
            await unitOfWork.SaveChangesAsync();
            return Ok(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var deletedUserId = await usersService.DeleteUser(userId);
            await unitOfWork.SaveChangesAsync();
            return Ok(deletedUserId);
        }
    }
}
