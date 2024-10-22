using Mapster;
using Microsoft.EntityFrameworkCore;
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


    public async Task<TournamentInfoDto?> GetTournamentResponse(Guid tournamentId)
    {
        var tournamentEntity = await GetTournament(tournamentId);
        return tournamentEntity.Adapt<TournamentInfoDto>();
    }

    public async Task<TournamentEntity?> GetTournament(Guid tournamentId)
    {
        var query = tournamentRepository.SingleResultQuery()
            .AndFilter(x => x.Id == tournamentId);
        var tournamentEntity = await tournamentRepository.FirstOrDefaultAsync(query);
        if (tournamentEntity == null)
        {
            logger.LogWarning("Tournament not found from database");
            return null;
        }

        logger.LogInformation($"Tournament {tournamentEntity.Id} is retrieved from database");
        return tournamentEntity;
    }

    public async Task<List<TournamentListItemDto>> GetAllTournaments()
    {
        var query = tournamentRepository.MultipleResultQuery();
        var tournaments = await tournamentRepository.SearchAsync(query);
        logger.LogInformation($"Tournaments count: {tournaments.Count}");
        return tournaments.Adapt<List<TournamentListItemDto>>();
    }

    public async Task<Guid> CreateTournament(CreateTournamentDto newTournament)
    {
        var newTournamentEntity = newTournament.Adapt<TournamentEntity>();
        var addedTournament = await tournamentRepository.AddAsync(newTournamentEntity);
        logger.LogInformation($"Tournament {newTournamentEntity.Id} created");
        return addedTournament.Id;
    }

    public async Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament)
    {
        var query = tournamentRepository.SingleResultQuery().AndFilter(x => x.Id == tournamentId);
        var userEntity = await tournamentRepository.FirstOrDefaultAsync(query);
        updateTournament.Adapt(userEntity);
        userEntity.UpdatedAt = DateTime.Now;
        logger.LogInformation($"User {tournamentId} updated");
        return tournamentId;
    }

    public async Task<Guid> DeleteTournament(Guid tournamentId)
    {
        var query = tournamentRepository.SingleResultQuery().AndFilter(x => x.Id == tournamentId);
        var teamEntity = await tournamentRepository.FirstOrDefaultAsync(query);
        tournamentRepository.Remove(teamEntity);
        logger.LogInformation($"Team {tournamentId} deleted");
        
        return tournamentId;
    }
}