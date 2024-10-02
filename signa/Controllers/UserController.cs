using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Entities;
using signa.Models;
using signa.Repositories;

namespace signa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;
        private readonly IMapper mapper;

        public UserController(UserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto user)
        {
            var userEntity = mapper.Map<CreateUserDto, UserEntity>(user);
            var userId = userRepository.Create(userEntity);
            return Ok(userId);
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid userId)
        {
            var userEntity = userRepository.GetById(userId).Result;
            if (userEntity == null)
                return NotFound();
            var userResponseDto = mapper.Map<UserEntity, UserResponseDto>(userEntity);
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
