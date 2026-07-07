using Azure.Storage.Blobs;
using CidadeInteligente.Domain.Services;

namespace CidadeInteligente.Infrastructure.Services;

public class AzureStorageService(BlobContainerClient blobContainerClient) : IFileStorage
{
    private readonly BlobContainerClient _blobContainerClient = blobContainerClient;

    public Task DeleteFileAsync(string fileName, CancellationToken cancellationToken) => _blobContainerClient.DeleteBlobIfExistsAsync(fileName, cancellationToken: cancellationToken);

    public async Task<string> UploadOrUpdateFileAsync(string fileName, Stream stream, CancellationToken cancellationToken)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken: cancellationToken);
        return fileName;
    }
}
