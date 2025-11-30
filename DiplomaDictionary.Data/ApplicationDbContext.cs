using DiplomaDictionary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaDictionary.Data;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Term> Terms { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public async Task AddRangeAsync<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : class
    {
        await Set<TEntity>().AddRangeAsync(entities);
    }

    public void AddRange<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : class
    {
        Set<TEntity>().AddRange(entities);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().UseCollation("NOCASE");
    }
}