﻿using ErrorOr;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using signa.Dto.team;
using signa.Entities;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Services;

public class TeamsService : ITeamsService
{
    private readonly ITeamRepository teamRepository;
    private readonly ILogger<TeamsService> logger;
    private readonly ITournamentsService tournamentsService;
    private readonly IUsersService usersService;

    public TeamsService(ITeamRepository teamRepository, ILogger<TeamsService> logger,
        ITournamentsService tournamentsService, IUsersService usersService)
    {
        this.teamRepository = teamRepository;
        this.logger = logger;
        this.tournamentsService = tournamentsService;
        this.usersService = usersService;
    }

    public async Task<ErrorOr<TeamEntity>> GetTeamEntity(Guid teamId)
    {
        var query = teamRepository.SingleResultQuery()
            .Include(x => 
                x.Include(y => y.Members)
                    .Include(y => y.Captain))
            .AndFilter(x => x.Id == teamId);
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);

        if (teamEntity == null)
            return Error.NotFound("General.NotFound", $"Can't find team by id {teamId}");
        
        return teamEntity;
    }

    public async Task<ErrorOr<List<TeamEntity>>> GetTeamEntitiesByIds(List<Guid> teamIds)
    {
        var query = teamRepository.MultipleResultQuery()
            .AndFilter(x => teamIds.Contains(x.Id))
            .Include(x =>
                x.Include(y => y.Members)
                    .Include(y => y.Captain));
        var teamEntities = await teamRepository.SearchAsync(query);
        if (teamEntities.Count < teamIds.Count)
        {
            var notFoundTeamIds = string.Join(" ", teamIds
                .Except(teamEntities.Select(x => x.Id).ToList())
                .Select(x => x.ToString()));
            return Error.NotFound("General.NotFound", $"Not found teams: {notFoundTeamIds}");
        }
        return teamEntities.ToList();
    }
    
    public async Task<ErrorOr<TeamEntity>> GetTeamEntityByCaptainId(Guid captainId)
    {
        var query = teamRepository.SingleResultQuery()
            .AndFilter(x => x.Captain.Id == captainId)
            .Include(x =>
                x.Include(y => y.Invites)
                    .ThenInclude(y => y.InvitedUser));
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);

        if (teamEntity == null)
            return Error.NotFound("General.NotFound", $"Can't find team by captain id {captainId}");
        
        return teamEntity;
    }

    public async Task<ErrorOr<TeamResponseDto?>> GetTeam(Guid teamId)
    {
        var teamEntity = await GetTeamEntity(teamId);
        if (teamEntity.IsError)
        {
            logger.LogWarning($"Team {teamId} not found from database");
            return teamEntity.FirstError;
        }

        logger.LogInformation($"Team {teamEntity.Value.Id} is retrieved from database");
        return teamEntity.Value.Adapt<TeamResponseDto>();
    }

    public async Task<ErrorOr<List<TeamResponseDto>>> GetTeamsByTournamentId(Guid tournamentId)
    {
        var query = teamRepository.MultipleResultQuery()
            .Include(x => 
                x.Include(y => y.Tournament)
                    .Include(y => y.Members))
            .AndFilter(x => x.Tournament.Id == tournamentId);
        var teamEntities = await teamRepository.SearchAsync(query);
        
        if (teamEntities.Count == 0)
        {
            logger.LogWarning($"Tournament {tournamentId} don't have any teams");
            return Error.NotFound("General.NotFound", $"Can't find any teams by tournament id {tournamentId}");
        }
        
        logger.LogInformation($"Tournament {tournamentId} has {teamEntities.Count} teams");
        var teams = teamEntities.Select(x => x.Adapt<TeamResponseDto>());
        return teams.ToList();
    }

    public async Task<ErrorOr<Guid>> CreateTeam(CreateTeamDto newTeam)
    {
        var capitan = await usersService.GetUser(newTeam.CaptainId);
        if(capitan.IsError)
            return capitan.FirstError;
        var tournament = await tournamentsService.GetTournament(newTeam.TournamentId);
        if (tournament.IsError)
            return tournament.FirstError;
        if (tournament.Value.MaxTeamsCount != 0 && tournament.Value.MaxTeamsCount < tournament.Value.Teams.Count)
            return Error.Failure("General.Failure", "На турнир больше нет мест.");
        
        var newTeamEntity = new TeamEntity
        {
            Title = newTeam.Title,
            Captain = capitan.Value,
            Members = [capitan.Value],
            Tournament = tournament.Value,
        };
        var addedTeamEntity = await teamRepository.AddAsync(newTeamEntity);
        return addedTeamEntity.Id;
    }

    public async Task<ErrorOr<Guid>> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam)
    {
        var query = teamRepository.SingleResultQuery().AndFilter(x => x.Id == teamId);
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);

        if (teamEntity == null)
            return Error.NotFound("General.NotFound", $"Can't find team by id {teamId}");
        
        updateTeam.Adapt(teamEntity);
        teamEntity.UpdatedAt = DateTime.Now;
        logger.LogInformation($"Team {teamId} updated");
        return teamId;
    }

    public async Task<ErrorOr<Guid>> DeleteTeam(Guid teamId)
    {
        var query = teamRepository.SingleResultQuery().AndFilter(x => x.Id == teamId);
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);
        
        if (teamEntity == null)
            return Error.NotFound("General.NotFound", $"Can't find team by id {teamId}");
        
        teamRepository.Remove(teamEntity);
        logger.LogInformation($"Team {teamId} deleted");
        
        return teamId;
    }
}

