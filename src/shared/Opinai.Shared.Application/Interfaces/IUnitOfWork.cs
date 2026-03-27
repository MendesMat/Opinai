namespace Opinai.Shared.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
