using System.Linq.Expressions;
using BankingSystem.Data.Context;
using BankingSystem.Data.Interfaces;
using BankingSystem.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data.Repositories;

public class Repository<T> : IRepository<T> where T : AuditableEntity
{
    protected readonly BankingDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    public Repository(BankingDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => !x.IsDeleted).Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 100);

        IQueryable<T> query = DbSet.Where(x => !x.IsDeleted);
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T> { Items = items, PageNumber = pageNumber, PageSize = pageSize, TotalCount = total };
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        entity.IsDeleted = true;
        Update(entity);
    }
}
