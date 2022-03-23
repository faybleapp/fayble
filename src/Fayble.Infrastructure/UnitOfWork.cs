using Fayble.Domain;

namespace Fayble.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly FaybleDbContext _context;

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    private bool disposed;

    public UnitOfWork(FaybleDbContext context)
    {
        _context = context;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}