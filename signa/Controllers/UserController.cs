using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;
using signa.Dto;
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
        private readonly ILogger<UserController> logger;

        public UserController(IUsersService usersService, ILogger<UserController> logger, IUnitOfWork unitOfWork)
        {
            this.usersService = usersService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto user)
        {
            var userId = await usersService.CreateUser(user);
            await unitOfWork.SaveChangesAsync();
            return Ok(userId);
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponseDto>> Get(Guid userId)
        {
            var userResponseDto =  await usersService.GetUser(userId);
            return userResponseDto != null ? Ok(userResponseDto) : NotFound();
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
