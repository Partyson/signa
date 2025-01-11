using EntityFrameworkCore.UnitOfWork.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.invite;
using signa.Entities;
using signa.Enums;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

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

    public async Task<List<InviteResponseDto>> GetInvitesResponse(Guid invitedUserId)
    {
        var query = inviteRepository.MultipleResultQuery()
            .AndFilter(x => x.InvitedUser.Id == invitedUserId)
            .Include(x => 
                x.Include(i => i.InvitedUser)
                    .Include(i => i.InviteTeam).ThenInclude(t => t.Captain));
        var invitesEntity = await inviteRepository.SearchAsync(query);
        return invitesEntity.Select(x => x.Adapt<InviteResponseDto>()).ToList();
    }

    public async Task<List<SentInviteDto>> GetSentInvites(Guid captainId)
    {
        var team = await teamsService.GetTeamEntityByCaptainId(captainId);
        return team.Invites.Select(x => x.Adapt<SentInviteDto>()).ToList();
    }

    public async Task<List<Guid>> CreateInvites(Guid teamId, List<Guid> invitedUserIds)
    {
        var teamEntity = await teamsService.GetTeamEntity(teamId);
        var invitedUsers = await usersService.GetUserEntitiesByIds(invitedUserIds);
        var invites = invitedUsers
            .Select(x => new InviteEntity{InvitedUser = x, InviteTeam = teamEntity}).ToList();
        await inviteRepository.AddRangeAsync(invites);
        return invites.Select(x => x.Id).ToList();
    }

    public async Task<Guid> AcceptInvite(Guid inviteId)
    {
        var query = inviteRepository.SingleResultQuery()
            .AndFilter(x => x.Id == inviteId);
        var inviteEntity = await inviteRepository.FirstOrDefaultAsync(query);
        inviteEntity.State = InviteState.Accepted;
        inviteEntity.UpdatedAt = DateTime.Now;
        inviteEntity.InviteTeam.Members.Add(inviteEntity.InvitedUser);
        return inviteEntity.Id;
    }

    public async Task<Guid> DiscardInvite(Guid inviteId)
    {
        var query = inviteRepository.SingleResultQuery()
            .AndFilter(x => x.Id == inviteId);
        var inviteEntity = await inviteRepository.FirstOrDefaultAsync(query);
        inviteEntity.State = InviteState.Discarded;
        inviteEntity.UpdatedAt = DateTime.Now;
        return inviteEntity.Id;
    }
}