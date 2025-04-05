using EntityFrameworkCore.UnitOfWork.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.invite;
using signa.Entities;
using signa.Enums;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;
using ErrorOr;

namespace signa.Services;

public class InvitesesService : IInvitesService
{
    private readonly IInviteRepository inviteRepository;
    private readonly ITeamsService teamsService;
    private readonly IUsersService usersService;

    public InvitesesService(IInviteRepository inviteRepository, ITeamsService teamsService, IUsersService usersService)
    {
        this.inviteRepository = inviteRepository;
        this.teamsService = teamsService;
        this.usersService = usersService;
    }

    public async Task<ErrorOr<List<InviteResponseDto>>> GetInvitesResponse(Guid invitedUserId)
    {
        var query = inviteRepository.MultipleResultQuery()
            .AndFilter(x => x.InvitedUser.Id == invitedUserId)
            .Include(x => 
                x.Include(i => i.InvitedUser)
                    .Include(i => i.InviteTeam).ThenInclude(t => t.Captain));
        var invitesEntity = await inviteRepository.SearchAsync(query);

        if (invitesEntity.Count == 0)
            return Error.NotFound("General.NotFound", $"Can't find any invites by user id {invitedUserId}");
        
        return invitesEntity.Select(x => x.Adapt<InviteResponseDto>()).ToList();
    }

    public async Task<ErrorOr<List<SentInviteDto>>> GetSentInvites(Guid captainId)
    {
        var team = await teamsService.GetTeamEntityByCaptainId(captainId);

        if (team.IsError)
            return team.FirstError;
        
        return team.Value.Invites.Select(x => x.Adapt<SentInviteDto>()).ToList();
    }

    public async Task<ErrorOr<List<Guid>>> CreateInvites(Guid teamId, List<Guid> invitedUserIds)
    {
        var teamEntity = await teamsService.GetTeamEntity(teamId);
        
        if (teamEntity.IsError)
            return teamEntity.FirstError;
        
        var invitedUsers = await usersService.GetUserEntitiesByIds(invitedUserIds);

        if (invitedUsers.IsError)
            return invitedUsers.FirstError;
        
        var invites = invitedUsers.Value
            .Select(x => new InviteEntity{InvitedUser = x, InviteTeam = teamEntity.Value}).ToList(); 
        await inviteRepository.AddRangeAsync(invites);
        return invites.Select(x => x.Id).ToList();
    }

    public async Task<ErrorOr<Guid>> AcceptInvite(Guid inviteId)
    {
        var query = inviteRepository.SingleResultQuery()
            .AndFilter(x => x.Id == inviteId);
        var inviteEntity = await inviteRepository.FirstOrDefaultAsync(query);

        if (inviteEntity == null)
            return Error.NotFound("General.NotFound",$"Can't find invite by id {inviteId}");
        
        inviteEntity.State = InviteState.Accepted;
        inviteEntity.UpdatedAt = DateTime.Now;
        inviteEntity.InviteTeam.Members.Add(inviteEntity.InvitedUser);
        return inviteEntity.Id;
    }

    public async Task<ErrorOr<Guid>> DiscardInvite(Guid inviteId)
    {
        var query = inviteRepository.SingleResultQuery()
            .AndFilter(x => x.Id == inviteId);
        var inviteEntity = await inviteRepository.FirstOrDefaultAsync(query);
        
        if (inviteEntity == null)
            return Error.NotFound("General.NotFound", $"Can't find invite by id {inviteId}"); 
        
        inviteEntity.State = InviteState.Discarded;
        inviteEntity.UpdatedAt = DateTime.Now;
        return inviteEntity.Id;
    }
}