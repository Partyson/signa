using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.group;
using signa.Interfaces.Services;

namespace signa.Controllers;

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
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto createGroupDto)
    {
        var groupIds = await groupsService.CreateGroups(createGroupDto);
        await unitOfWork.SaveChangesAsync();
        return Ok(groupIds);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<GroupResponseDto>>> GetGroupsByTournamentId([FromQuery] Guid tournamentId)
    {
        var groups = await groupsService.GetGroupsByTournamentId(tournamentId);
        return Ok(groups);
    }
}