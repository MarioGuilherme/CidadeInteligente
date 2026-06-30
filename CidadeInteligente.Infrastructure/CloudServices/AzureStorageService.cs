using Azure.Storage.Blobs;
using CidadeInteligente.Core.Services;

namespace CidadeInteligente.Infrastructure.CloudServices;

public class AzureStorageService(BlobContainerClient blobContainerClient) : IFileStorage
{
    private readonly BlobContainerClient _blobContainerClient = blobContainerClient;

    public Task DeleteFileAsync(string fileName) => _blobContainerClient.DeleteBlobIfExistsAsync(fileName);

    public async Task<string> UploadOrUpdateFileAsync(string fileName, Stream stream)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream, overwrite: true);
        return fileName;
    }
}
