﻿using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Dto.team;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class TeamRepository(ApplicationDbContext context) : Repository<TeamEntity>(context), ITeamRepository;