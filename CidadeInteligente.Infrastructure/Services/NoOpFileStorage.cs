using CidadeInteligente.Domain.Services;

namespace CidadeInteligente.Infrastructure.Services;

public class NoOpFileStorage : IFileStorage
{
    public Task DeleteFileAsync(string fileName, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task<string> UploadOrUpdateFileAsync(string fileName, Stream stream, CancellationToken cancellationToken) => Task.FromResult(fileName);
}
