using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.invite;
using signa.Enums;
using signa.Interfaces.Services;

namespace signa.Controllers;

[Route("invites")]
public class InviteController : ControllerBase
{
    private readonly IInviteService inviteService;
    private readonly IUnitOfWork unitOfWork;


    public InviteController(IInviteService inviteService, IUnitOfWork unitOfWork)
    {
        this.inviteService = inviteService;
        this.unitOfWork = unitOfWork;
    }

    [Authorize]
    [HttpGet("{invitedUserId}")]
    public async Task<ActionResult<List<InviteResponseDto>>> GetUsersInvite(Guid invitedUserId)
    {
        var invites =  await inviteService.GetInvitesResponse(invitedUserId);
        return invites.Count != 0 ? Ok(invites) : NotFound();
    }

    [Authorize]
    [HttpGet("{captainId}/sent")]
    public async Task<ActionResult<List<SentInviteDto>>> GetSentInvites(Guid captainId)
    {
        var invites = await inviteService.GetSentInvites(captainId);
        return invites.Count != 0 ? Ok(invites) : NotFound();
    }

    [Authorize]
    [HttpPost("{teamId}")]
    public async Task<IActionResult> Create(Guid teamId, [FromBody] List<Guid> invitedUsers)
    {
        var invitesId = await inviteService.CreateInvites(teamId, invitedUsers);
        await unitOfWork.SaveChangesAsync();
        return Ok(invitesId);
    }
    
    [Authorize]
    [HttpPatch("{inviteId}/accept")]
    public async Task<IActionResult> Accept(Guid inviteId)
    {
        var acceptedInviteId = await inviteService.AcceptInvite(inviteId);
        return Ok(acceptedInviteId);
    }

    [Authorize]
    [HttpPatch("{inviteId}/discard")]
    public async Task<IActionResult> Discard(Guid inviteId)
    {
        var discardedInviteId = await inviteService.DiscardInvite(inviteId);
        return Ok(discardedInviteId);
    }
}