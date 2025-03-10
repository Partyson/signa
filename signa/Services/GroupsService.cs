using System.Text.RegularExpressions;
using ErrorOr;
using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.group;
using signa.Entities;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Services;

public class GroupsService : IGroupsService
{
    private readonly IGroupRepository groupRepository;
    private readonly ITournamentsService tournamentsService;
    private readonly ITeamsService teamsService;
    

    public GroupsService(IGroupRepository groupRepository,
        ITournamentsService tournamentsService, ITeamsService teamsService)
    {
        this.groupRepository = groupRepository;
        this.tournamentsService = tournamentsService;
        this.teamsService = teamsService;
    }

    public async Task<ErrorOr<List<Guid>>> CreateGroups(CreateGroupDto createGroupDto)
    {
        var tournament = await tournamentsService.GetTournament(createGroupDto.TournamentId);
        if (tournament.IsError)
            return tournament.FirstError;
        var groups = Enumerable.Range(0, createGroupDto.GroupCount)
            .Select(x => new GroupEntity{Tournament = tournament.Value, Title = $"Группа {x + 1}"}).ToList();
        var teams = await teamsService.GetTeamEntitiesByIds(createGroupDto.TeamsIds);
        var groupIndex = 0;
        foreach (var teamEntity in teams.Value)
        {
            groups[groupIndex].Teams.Add(teamEntity);
            groupIndex = groupIndex + 1 == createGroupDto.GroupCount ? 0 : groupIndex + 1;
        }
        
        await groupRepository.AddRangeAsync(groups);
        return groups.Select(x => x.Id).ToList();
    }

    public async Task<List<GroupResponseDto>> GetGroupsByTournamentId(Guid tournamentId)
    {
        var query = groupRepository.MultipleResultQuery()
            .AndFilter(x => x.Tournament.Id == tournamentId)
            .Include(x => 
                x.Include(g => g.Teams)
                    .ThenInclude(g => g.Matches)
                    .ThenInclude(m => m.Group));
        var groups = await groupRepository.SearchAsync(query);
        return groups.Select(g => g.Adapt<GroupResponseDto>()).ToList();
    }
}