﻿using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces.Repositories;

namespace signa.Repositories;

public class TeamRepository(ApplicationDbContext context) : Repository<TeamEntity>(context), ITeamRepository;