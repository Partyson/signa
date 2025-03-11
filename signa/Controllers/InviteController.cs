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
        var invitesId = await invitesService.CreateInvites(teamId, invitedUsers);
        await unitOfWork.SaveChangesAsync();
        return Ok(invitesId);
    }

    [Authorize]
    [HttpGet("{invitedUserId}")]
    public async Task<ActionResult<List<InviteResponseDto>>> GetUsersInvite(Guid invitedUserId)
    {
        var invites =  await invitesService.GetInvitesResponse(invitedUserId);
        return invites.Count != 0 ? Ok(invites) : NotFound();
    }

    [Authorize]
    [HttpGet("{captainId}/sent")]
    public async Task<ActionResult<List<SentInviteDto>>> GetSentInvites(Guid captainId)
    {
        var invites = await invitesService.GetSentInvites(captainId);
        return invites.Count != 0 ? Ok(invites) : NotFound();
    }
    
    [Authorize]
    [HttpPatch("{inviteId}/accept")]
    public async Task<IActionResult> Accept(Guid inviteId)
    {
        var acceptedInviteId = await invitesService.AcceptInvite(inviteId);
        return Ok(acceptedInviteId);
    }

    [Authorize]
    [HttpPatch("{inviteId}/discard")]
    public async Task<IActionResult> Discard(Guid inviteId)
    {
        var discardedInviteId = await invitesService.DiscardInvite(inviteId);
        return Ok(discardedInviteId);
    }
}