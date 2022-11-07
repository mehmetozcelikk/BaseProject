﻿using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Repositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Concretes;

public class RefreshTokenManager : EfRepositoryBase<RefreshToken, BaseDbContext>, IRefreshTokenService
{
    public RefreshTokenManager(BaseDbContext context) : base(context)
    {
    }
}
