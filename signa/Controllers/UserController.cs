using Mapster;
using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Entities;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto user)
        {
            var userEntity = user.Adapt<UserEntity>();
            var userId = await userRepository.Create(userEntity, user.Password);
            return Ok(userId);
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid userId)
        {
            var userEntity = userRepository.GetById(userId).Result;
            if (userEntity == null)
                return NotFound();
            var userResponseDto = userEntity.Adapt<UserResponseDto>();
            return Ok(userResponseDto);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto user)
        {
            await userRepository.Update(userId, user);
            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            await userRepository.Delete(userId);
            return Ok();
        }
    }
}
