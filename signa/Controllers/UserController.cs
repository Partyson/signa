using System.Security.Claims;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.user;
using signa.Extensions;
using signa.Interfaces.Services;
using signa.Validators;

namespace signa.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUsersService usersService;
        private readonly UserPassValidator userPassValidator;

        public UserController(IUsersService usersService, IUnitOfWork unitOfWork, UserPassValidator userPassValidator)
        {
            this.usersService = usersService;
            this.unitOfWork = unitOfWork;
            this.userPassValidator = userPassValidator;
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult<UserResponseDto>> Get([FromRoute] Guid userId)
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

        [Authorize(Roles = "Admin")]
        [HttpPatch("{userId}/update")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserDto user)
        {
            var id = await usersService.UpdateUser(userId, user);
            
            if (id.IsError)
                return Problem(id.FirstError.Description, statusCode: id.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(id.Value);
        }
        
        [Authorize]
        [HttpPatch("change-password")]
        public async Task<IActionResult> UpdateUserPass([FromBody] UpdateUserPassDto user)
        {
            var validationResult = await userPassValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToString(Environment.NewLine));
                
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null)
                return Problem("Невозможно определить ваш ID.", statusCode: StatusCodes.Status401Unauthorized);

            if (!Guid.TryParse(currentUserIdClaim, out var currentUserId))
                return Problem("Неверный токен или ID пользователя.", statusCode: StatusCodes.Status401Unauthorized);
            
            var id = await usersService.UpdateUserPass(currentUserId, user);
            
            if (id.IsError)
                return Problem(id.FirstError.Description, statusCode: id.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(id.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}/delete")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            var deletedUserId = await usersService.DeleteUser(userId);
            
            if (deletedUserId.IsError)
                return Problem(deletedUserId.FirstError.Description, statusCode: deletedUserId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(deletedUserId.Value);
        }
    }
}
