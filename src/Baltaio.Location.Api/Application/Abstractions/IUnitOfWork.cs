namespace Baltaio.Location.Api.Application.Abstractions;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}
