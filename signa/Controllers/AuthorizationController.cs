﻿using System.Security.Claims;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.user;
using signa.Extensions;
using signa.Validators;
using IAuthorizationService = signa.Interfaces.Services.IAuthorizationService;

namespace signa.Controllers;

[ApiController]
[Route("auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService authorizationService;
    private readonly IUnitOfWork unitOfWork;
    private readonly UserValidator validator;
    public AuthorizationController(IAuthorizationService authorizationService, IUnitOfWork unitOfWork,
        UserValidator validator)
    {
        this.authorizationService = authorizationService;
        this.unitOfWork = unitOfWork;
        this.validator = validator;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] CreateUserDto user)
    {
        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToString(Environment.NewLine));
        
        var token = await authorizationService.RegisterUser(user);
        if (token.IsError)
            return Problem(token.FirstError.Description, statusCode: token.FirstError.Type.ToStatusCode());
        await unitOfWork.SaveChangesAsync();
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddHours(12), // Куки будет жить 12 часов
            HttpOnly = true, // Защита от доступа через JavaScript
            Secure = false,
            MaxAge = TimeSpan.FromDays(7),
            SameSite = SameSiteMode.Strict // Защита от CSRF-атак
        };
        Response.Cookies.Append("token", token.Value, cookieOptions);
        return Ok(token.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var token = await authorizationService.LoginUser(userLoginRequest.Email, userLoginRequest.Password);
        if(token.IsError)
            return Problem(token.FirstError.Description, statusCode: token.FirstError.Type.ToStatusCode());
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddHours(12), // Куки будет жить 12 часов
            HttpOnly = true, // Защита от доступа через JavaScript
            Secure = false,
            MaxAge = TimeSpan.FromDays(7),
            SameSite = SameSiteMode.Strict // Защита от CSRF-атак
        };
        Response.Cookies.Append("token", token.Value, cookieOptions);
        return Ok(token.Value);
    }
    
    [Authorize]
    [HttpGet("get-user-id")]
    public ActionResult<Guid> GetUserIdFromToken()
    {
        var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(Guid.Parse(userId));
    }

    [Authorize]
    [HttpGet("get-role")]
    public ActionResult<string> GetRoleFromToken()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role == "Admin") // Вернем любую роль кроме админа, для безопасности (?)
            role = "User";
        return Ok(role);
    }

    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok("Logged out");
    }
}