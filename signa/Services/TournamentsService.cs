﻿using Mapster;
using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.tournament;
using signa.Entities;
using signa.Interfaces;

namespace signa.Services;

public class TournamentsService : ITournamentsService
{
    private readonly ITournamentRepository tournamentRepository;
    private readonly ILogger<TournamentsService> logger;

    public TournamentsService(ITournamentRepository tournamentRepository, ILogger<TournamentsService> logger)
    {
        this.tournamentRepository = tournamentRepository;
        this.logger = logger;
    }


    public async Task<TournamentInfoDto?> GetTournament(Guid tournamentId)
    {
        var tournament = await tournamentRepository.Get(tournamentId);
        return tournament.Adapt<TournamentInfoDto>();
    }

    public async Task<List<TournamentListItemDto>> GetAllTournaments()
    {
        var tournaments = await tournamentRepository.GetAll();
        logger.LogInformation($"Tournaments: {tournaments.Count}");
        return tournaments.Adapt<List<TournamentListItemDto>>();
    }

    public async Task<List<MatchResponseDto>> GetMatches(Guid tournamentId)
    {
        var tournament = await tournamentRepository.Get(tournamentId);
        return tournament.Matches.Adapt<List<MatchResponseDto>>();
    }

    public async Task<List<TeamResponseDto>> GetTeams(Guid tournamentId)
    {
        var tournament = await tournamentRepository.Get(tournamentId);
        return tournament.Teams.Adapt<List<TeamResponseDto>>();
    }

    public async Task<Guid> CreateTournament(CreateTournamentDto newTournament)
    {
        var tournamentEntity = newTournament.Adapt<TournamentEntity>();
        tournamentEntity.Id = Guid.NewGuid();
        tournamentEntity.CreatedAt = DateTime.Now;
        tournamentEntity.UpdatedAt = tournamentEntity.CreatedAt;
        var id = await tournamentRepository.Create(tournamentEntity);
        logger.LogInformation($"Tournament {tournamentEntity.Id} created");
        return id;
    }

    public async Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament)
    {
        var newTournamentEntity = updateTournament.Adapt<TournamentEntity>();
        newTournamentEntity.Id = tournamentId;
        var updatedTournamentId = await tournamentRepository.Update(newTournamentEntity);
        logger.LogInformation($"Tournament {updatedTournamentId} updated");
        return updatedTournamentId;
    }

    public async Task<Guid> DeleteTournament(Guid tournamentId)
    {
        var id = await tournamentRepository.Delete(tournamentId);
        logger.LogInformation($"Tournament {id} deleted");
        
        return id;
    }
}