using Mapster;
using signa.Dto;
using signa.Entities;
using signa.Interfaces;

namespace signa.Services;

public class TournamentsService : ITournamentsService
{
    private readonly ITournamentRepository tournamentRepository;

    public TournamentsService(ITournamentRepository tournamentRepository)
    {
        this.tournamentRepository = tournamentRepository;
    }


    public async Task<TournamentResponseDto?> GetTournament(Guid tournamentId)
    {
        var tournamentEntity = await tournamentRepository.Get(tournamentId);
        return tournamentEntity.Adapt<TournamentResponseDto>();
    }

    public async Task<List<TournamentResponseDto>> GetAllTournaments()
    {
        var tournaments = await tournamentRepository.GetAll();
        return tournaments.Adapt<List<TournamentResponseDto>>();
    }

    public async Task<Guid> CreateTournament(CreateTournamentDto newTournament)
    {
        var tournamentEntity = newTournament.Adapt<TournamentEntity>();
        tournamentEntity.Id = Guid.NewGuid();
        tournamentEntity.CreatedAt = DateTime.Now;
        tournamentEntity.UpdatedAt = tournamentEntity.CreatedAt;
        var id = await tournamentRepository.Create(tournamentEntity);
        return id;
    }

    public async Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament)
    {
        var newTournamentEntity = updateTournament.Adapt<TournamentEntity>();
        newTournamentEntity.Id = Guid.NewGuid();
        var updatedTournamentId = await tournamentRepository.Update(newTournamentEntity);
        return updatedTournamentId;
    }

    public Task<Guid> DeleteTournament(Guid tournamentId)
    {
        throw new NotImplementedException();
    }
}