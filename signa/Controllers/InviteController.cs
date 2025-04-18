using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.invite;
using signa.Enums;
using signa.Extensions;
using signa.Interfaces.Services;

namespace signa.Controllers;

[ApiController]
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
    [HttpPost("{teamId}/create")]
    public async Task<IActionResult> Create([FromRoute] Guid teamId, [FromBody] List<Guid> invitedUsers)
    {
        var invitesId = await invitesService.CreateInvites(teamId, invitedUsers);
        
        if (invitesId.IsError)
            return Problem(invitesId.FirstError.Description,
                statusCode: invitesId.FirstError.Type.ToStatusCode());
        
        await unitOfWork.SaveChangesAsync();
        return Ok(invitesId.Value);
    }

    [Authorize]
    [HttpGet("{invitedUserId}")]
    public async Task<ActionResult<List<InviteResponseDto>>> GetUsersInvite([FromRoute] Guid invitedUserId)
    {
        var invites =  await invitesService.GetInvitesResponse(invitedUserId);
        
        if (invites.IsError)
            return Problem(invites.FirstError.Description,
                statusCode: invites.FirstError.Type.ToStatusCode());

        return Ok(invites.Value);
    }

    [Authorize]
    [HttpGet("{captainId}/sent")]
    public async Task<ActionResult<List<SentInviteDto>>> GetSentInvites([FromRoute] Guid captainId)
    {
        var invites = await invitesService.GetSentInvites(captainId);
        
        if (invites.IsError)
            return Problem(invites.FirstError.Description,
                statusCode: invites.FirstError.Type.ToStatusCode());
        
        return Ok(invites.Value);
    }
    
    [Authorize]
    [HttpPatch("{inviteId}/accept")]
    public async Task<IActionResult> Accept([FromRoute] Guid inviteId)
    {
        var acceptedInviteId = await invitesService.AcceptInvite(inviteId);
        
        if (acceptedInviteId.IsError)
            return Problem(acceptedInviteId.FirstError.Description,
                statusCode: acceptedInviteId.FirstError.Type.ToStatusCode());
        
        return Ok(acceptedInviteId.Value);
    }

    [Authorize]
    [HttpPatch("{inviteId}/discard")]
    public async Task<IActionResult> Discard([FromRoute] Guid inviteId)
    {
        var discardedInviteId = await invitesService.DiscardInvite(inviteId);
        
        if (discardedInviteId.IsError)
            return Problem(discardedInviteId.FirstError.Description,
                statusCode: discardedInviteId.FirstError.Type.ToStatusCode());
        
        return Ok(discardedInviteId.Value);
    }
}