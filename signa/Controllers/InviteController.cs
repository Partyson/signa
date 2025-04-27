using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.invite;
using signa.Enums;
using signa.Extensions;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Controllers;

[Route("invites")]
public class InviteController : ControllerBase
{
    private readonly IInvitesService invitesService;
    private readonly IUnitOfWork unitOfWork;


    public InviteController(IInvitesService invitesService, IUnitOfWork unitOfWork)
    {
        this.invitesService = invitesService;
        this.unitOfWork = unitOfWork;
    }
    
    [Authorize]
    [HttpPost("{teamId}")]
    public async Task<IActionResult> Create(Guid teamId, [FromBody] List<Guid> invitedUsers)
    {
        var currentUserId = User.GetUserId();

        if (currentUserId.IsError)
            return Problem(currentUserId.FirstError.Description,
                statusCode: currentUserId.FirstError.Type.ToStatusCode());
        
        var invitesId = await invitesService.CreateInvites(teamId, invitedUsers, currentUserId.Value);
        
        if (invitesId.IsError)
            return Problem(invitesId.FirstError.Description,
                statusCode: invitesId.FirstError.Type.ToStatusCode());
        
        await unitOfWork.SaveChangesAsync();
        return Ok(invitesId.Value);
    }

    [Authorize]
    [HttpGet("{invitedUserId}")]
    public async Task<ActionResult<List<InviteResponseDto>>> GetUsersInvite(Guid invitedUserId)
    {
        var currentUserId = User.GetUserId();
        
        if (currentUserId.IsError)
            return Problem(currentUserId.FirstError.Description,
                statusCode: currentUserId.FirstError.Type.ToStatusCode());
        
        if (currentUserId.Value != invitedUserId)
            return Problem("Нельзя получить инвайты другого пользователя.", statusCode: StatusCodes.Status403Forbidden);
        
        var invites =  await invitesService.GetInvitesResponse(invitedUserId);
        
        if (invites.IsError)
            return Problem(invites.FirstError.Description,
                statusCode: invites.FirstError.Type.ToStatusCode());

        return Ok(invites.Value);
    }

    [Authorize]
    [HttpGet("{captainId}/sent")]
    public async Task<ActionResult<List<SentInviteDto>>> GetSentInvites(Guid captainId)
    {
        var currentUserId = User.GetUserId();
        
        if (currentUserId.IsError)
            return Problem(currentUserId.FirstError.Description,
                statusCode: currentUserId.FirstError.Type.ToStatusCode());
        
        if (currentUserId.Value != captainId)
            return Problem("Нельзя получить отправленные инвайты если вы не капитан.", statusCode: StatusCodes.Status403Forbidden);
        
        var invites = await invitesService.GetSentInvites(captainId);
        
        if (invites.IsError)
            return Problem(invites.FirstError.Description,
                statusCode: invites.FirstError.Type.ToStatusCode());
        
        return Ok(invites.Value);
    }
    
    [Authorize]
    [HttpPatch("{inviteId}/accept")]
    public async Task<IActionResult> Accept(Guid inviteId)
    {
        var currentUserId = User.GetUserId();
        
        if (currentUserId.IsError)
            return Problem(currentUserId.FirstError.Description,
                statusCode: currentUserId.FirstError.Type.ToStatusCode());
        
        var acceptedInviteId = await invitesService.AcceptInvite(inviteId, currentUserId.Value);
        
        if (acceptedInviteId.IsError)
            return Problem(acceptedInviteId.FirstError.Description,
                statusCode: acceptedInviteId.FirstError.Type.ToStatusCode());
        
        return Ok(acceptedInviteId.Value);
    }

    [Authorize]
    [HttpPatch("{inviteId}/discard")]
    public async Task<IActionResult> Discard(Guid inviteId)
    {
        var currentUserId = User.GetUserId();
        
        if (currentUserId.IsError)
            return Problem(currentUserId.FirstError.Description,
                statusCode: currentUserId.FirstError.Type.ToStatusCode());
        
        var discardedInviteId = await invitesService.DiscardInvite(inviteId, currentUserId.Value);
        
        if (discardedInviteId.IsError)
            return Problem(discardedInviteId.FirstError.Description,
                statusCode: discardedInviteId.FirstError.Type.ToStatusCode());
        
        return Ok(discardedInviteId.Value);
    }
}