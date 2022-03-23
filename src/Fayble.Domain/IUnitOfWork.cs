namespace Fayble.Domain;

public interface IUnitOfWork : IDisposable
{
    Task Commit();
}