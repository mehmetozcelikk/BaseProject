﻿using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Repositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Concretes;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, BaseDbContext>, IUserOperationClaimService
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}

