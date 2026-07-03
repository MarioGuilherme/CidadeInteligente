using CidadeInteligente.Core.Services;

namespace CidadeInteligente.Infrastructure.CloudServices;

public class AzureStorageServiceTest : IFileStorage
{
    public Task DeleteFileAsync(string fileName, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<string> UploadOrUpdateFileAsync(string fileName, Stream stream, CancellationToken cancellationToken) => fileName;
}
