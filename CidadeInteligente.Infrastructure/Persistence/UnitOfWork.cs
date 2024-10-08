﻿using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CidadeInteligente.Infrastructure.Persistence;

public class UnitOfWork(
    CidadeInteligenteDbContext dbContext,
    IAreaRepository areas,
    ICourseRepository courses,
    IProjectRepository projects,
    IUserRepository users) : IUnitOfWork {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public IAreaRepository Areas { get; } = areas;
    public ICourseRepository Courses { get; } = courses;
    public IProjectRepository Projects { get; } = projects;
    public IUserRepository Users { get; } = users;

    public Task<int> CompleteAsync() => this._dbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync() => this._transaction = await this._dbContext.Database.BeginTransactionAsync();

    public async Task CommitAsync() {
        try {
            await this._transaction!.CommitAsync();
        } catch (Exception) {
            await this._transaction!.RollbackAsync();
            throw;
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing)
            this._dbContext.Dispose();
    }
}