using Azure.Storage.Blobs;
using CidadeInteligente.Core.Services;

namespace CidadeInteligente.Infrastructure.CloudServices;

public class AzureStorageService : IFileStorage {
    private readonly string _connectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString")!;
    private readonly string _containerName = Environment.GetEnvironmentVariable("AzureStorageContainerName")!;

    public async Task DeleteFileAsync(string fileName) {
        BlobContainerClient blobContainerClient = new(this._connectionString, this._containerName);
        await blobContainerClient.DeleteBlobIfExistsAsync(fileName);
    }

    public async Task<string> UploadFileAsync(string fileName, byte[] bytes) {
        BlobContainerClient blobContainerClient = new(this._connectionString, this._containerName);
        await blobContainerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        await blobContainerClient.UploadBlobAsync(fileName, new BinaryData(bytes));

        return fileName;
    }
}