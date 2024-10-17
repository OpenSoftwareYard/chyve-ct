using System;

namespace Persistence.Data;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T entity);
    Task<T?> Update(T entity);
    Task<T?> Update(Guid id, Action<T> updateAction);
    Task<T?> Delete(Guid id);

    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
}
