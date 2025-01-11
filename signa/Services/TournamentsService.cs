using ErrorOr;
using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.tournament;
using signa.Entities;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Services;

public class TournamentsService : ITournamentsService
{
    private readonly ITournamentRepository tournamentRepository;
    private readonly ILogger<TournamentsService> logger;

    public TournamentsService(ITournamentRepository tournamentRepository,
        ILogger<TournamentsService> logger)
    {
        this.tournamentRepository = tournamentRepository;
        this.logger = logger;
    }


    public async Task<TournamentInfoDto?> GetTournamentResponse(Guid tournamentId)
    {
        var tournamentEntity = await GetTournament(tournamentId);
        return tournamentEntity.Adapt<TournamentInfoDto>();
    }
    
    public async Task<ErrorOr<TournamentEntity>> GetTournament(Guid tournamentId)
    {
        var query = tournamentRepository.SingleResultQuery()
            .Include(x => 
                x.Include(t => t.Teams)
                    .ThenInclude(team=>team.Members)
                    .Include(t=>t.Organizers)
                    .Include(t => t.Groups)
                    .ThenInclude(g => g.Teams)
                    .Include(t=>t.Matches)
                    .ThenInclude(m=> m.Teams))
            .AndFilter(x => x.Id == tournamentId);
        var tournamentEntity = await tournamentRepository.FirstOrDefaultAsync(query);
        if (tournamentEntity == null)
        {
            logger.LogWarning($"Tournament {tournamentId} not found from database");
            return Error.NotFound("General.NotFound",
                $"Tournament {tournamentId} not found from database");
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
        logger.LogInformation($"Tournament {tournamentId} updated");
        return tournamentId;
    }

    public async Task<Guid> DeleteTournament(Guid tournamentId)
    {
        var query = tournamentRepository.SingleResultQuery().AndFilter(x => x.Id == tournamentId);
        var teamEntity = await tournamentRepository.FirstOrDefaultAsync(query);
        tournamentRepository.Remove(teamEntity);
        logger.LogInformation($"Tournament {tournamentId} deleted");
        
        return tournamentId;
    }
}