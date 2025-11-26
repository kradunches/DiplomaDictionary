using Microsoft.EntityFrameworkCore;

namespace DiplomaDictionary.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<T> Set<T>() where T : class;

    Task AddRangeAsync<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : class;

    void AddRange<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : class;
}