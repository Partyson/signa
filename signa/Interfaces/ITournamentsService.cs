﻿using signa.Dto;
using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.tournament;
using signa.Entities;

namespace signa.Interfaces;

public interface ITournamentsService
{
    Task<TournamentInfoDto?> GetTournamentResponse(Guid tournamentId);
    Task<TournamentEntity?> GetTournament(Guid tournamentId);
    Task<List<TournamentListItemDto>> GetAllTournaments();
    Task<Guid> CreateTournament(CreateTournamentDto newTournament);
    Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament);
    Task<Guid> DeleteTournament(Guid tournamentId);
}