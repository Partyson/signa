using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.user;
using signa.Extensions;
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
            var userResponseDto = await usersService.GetUserResponse(userId);

            if (userResponseDto.IsError)
                return Problem(userResponseDto.FirstError.Description, statusCode: userResponseDto.FirstError.Type.ToStatusCode());
                
            return Ok(userResponseDto.Value);
        }
        

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<List<UserSearchItemDto>>> GetAllUsersByPrefix([FromQuery] string prefix)
        {
            var foundUsers = await usersService.GetUsersByPrefix(prefix);

            if (foundUsers.IsError)
                return Problem(foundUsers.FirstError.Description, statusCode: foundUsers.FirstError.Type.ToStatusCode());
            
            return Ok(foundUsers.Value);
        }

        [Authorize]
        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto user)
        {
            var id = await usersService.UpdateUser(userId, user);
            
            if (id.IsError)
                return Problem(id.FirstError.Description, statusCode: id.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(id.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var deletedUserId = await usersService.DeleteUser(userId);
            
            if (deletedUserId.IsError)
                return Problem(deletedUserId.FirstError.Description, statusCode: deletedUserId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(deletedUserId.Value);
        }
    }
}
