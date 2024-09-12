using Azure.Storage.Blobs;
using CidadeInteligente.Core.Services;

namespace CidadeInteligente.Infrastructure.CloudServices;

public class AzureStorageService(string connectionString, string containerName) : IFileStorage {
    private readonly BlobContainerClient _blobContainerClient = new(connectionString, containerName);

    public async Task DeleteFileAsync(string fileName) => await this._blobContainerClient.DeleteBlobIfExistsAsync(fileName);

    public async Task<string> UploadOrUpdateFileAsync(string fileName, byte[] bytes) {
        BlobClient blobClient = this._blobContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(new BinaryData(bytes), overwrite: true);
        return fileName;
    }
}