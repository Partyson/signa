using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.user;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService, IUnitOfWork unitOfWork)
        {
            this.usersService = usersService;
            this.unitOfWork = unitOfWork;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserDto user)
        {
            var token = await usersService.CreateUser(user);
            await unitOfWork.SaveChangesAsync();
            Response.Cookies.Append("token", token);
            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var token = await usersService.LoginUser(userLoginRequest.Email, userLoginRequest.Password);
            Response.Cookies.Append("token", token);
            return Ok(token);
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponseDto>> Get(Guid userId)
        {
            var userResponseDto =  await usersService.GetUserResponse(userId);
            return userResponseDto != null ? Ok(userResponseDto) : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<List<UserSearchItemDto>>> GetAllUsersByPrefix([FromQuery] string prefix)
        {
            var foundUsers = await usersService.GetUsersByPrefix(prefix);
            return Ok(foundUsers);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto user)
        {
            var id = await usersService.UpdateUser(userId, user);
            await unitOfWork.SaveChangesAsync();
            return Ok(id);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var deletedUserId = await usersService.DeleteUser(userId);
            await unitOfWork.SaveChangesAsync();
            return Ok(deletedUserId);
        }
    }
}
