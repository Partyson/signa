using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.group;
using signa.Extensions;
using signa.Interfaces.Services;

namespace signa.Controllers;

[ApiController]
[Route("group")]
public class GroupController : ControllerBase
{
    private readonly IGroupsService groupsService;
    private IUnitOfWork unitOfWork;

    public GroupController(IGroupsService groupsService, IUnitOfWork unitOfWork)
    {
        this.groupsService = groupsService;
        this.unitOfWork = unitOfWork;
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto createGroupDto)
    {
        var groupIds = await groupsService.CreateGroups(createGroupDto);

        if (groupIds.IsError)
            return Problem(groupIds.FirstError.Description,
                statusCode: groupIds.FirstError.Type.ToStatusCode());
        await unitOfWork.SaveChangesAsync();
        return Ok(groupIds.Value);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<GroupResponseDto>>> GetGroupsByTournamentId([FromQuery] Guid tournamentId)
    {
        var groups = await groupsService.GetGroupsByTournamentId(tournamentId);

        if (groups.IsError)
            return Problem(groups.FirstError.Description,  statusCode: groups.FirstError.Type.ToStatusCode());
            
        return Ok(groups.Value);
    }
}