using System.Security.Claims;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.user;
using IAuthorizationService = signa.Interfaces.Services.IAuthorizationService;

namespace signa.Controllers;

[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService authorizationService;
    private readonly IUnitOfWork unitOfWork;

    public AuthorizationController(IAuthorizationService authorizationService, IUnitOfWork unitOfWork)
    {
        this.authorizationService = authorizationService;
        this.unitOfWork = unitOfWork;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] CreateUserDto user)
    {
        var token = await authorizationService.RegisterUser(user);
        await unitOfWork.SaveChangesAsync();
        Response.Cookies.Append("token", token);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var token = await authorizationService.LoginUser(userLoginRequest.Email, userLoginRequest.Password);
        Response.Cookies.Append("token", token);
        return Ok(token);
    }
    
    [Authorize]
    [HttpGet("get-user-id")]
    public ActionResult<Guid> GetUserIdFromToken()
    {
        var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(Guid.Parse(userId));
    }

    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok("Logged out");
    }
}