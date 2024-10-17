using System;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;

public class GenericRepository<T>(ChyveContext context) : IGenericRepository<T> where T : class
{
    protected readonly ChyveContext _context = context;

    public async Task<T> Add(T entity)
    {
        var addedEntity = (await _context.Set<T>().AddAsync(entity)).Entity;
        await _context.SaveChangesAsync();
        return addedEntity;
    }

    public async Task BeginTransaction()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task<T?> Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity == null)
        {
            return null;
        }

        var returned = _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();

        return returned.Entity;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task RollbackTransaction()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task<T?> Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T?> Update(Guid id, Action<T> updateAction)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        updateAction(entity);
        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return entity;
    }
}
