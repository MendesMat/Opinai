using Microsoft.EntityFrameworkCore;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Infrastructure.Persistence;

public class UnitOfWork<T>(T context) : IUnitOfWork
    where T : DbContext
{
    private readonly T _context = context;
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
