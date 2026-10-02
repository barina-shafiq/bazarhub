using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebProject.Data;
using WebProject.Interfaces;

namespace WebProject.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context = context;
    protected readonly DbSet<T> Set = context.Set<T>();

    public IQueryable<T> Query() => Set.AsQueryable();

    public async Task<T?> GetByIdAsync(int id) => await Set.FindAsync(id);

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = predicate is null ? Set : Set.Where(predicate);
        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity) => await Set.AddAsync(entity);

    public void Update(T entity) => Set.Update(entity);

    public void Remove(T entity) => Set.Remove(entity);

    public async Task SaveChangesAsync() => await Context.SaveChangesAsync();
}
