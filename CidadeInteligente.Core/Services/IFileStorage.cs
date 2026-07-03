namespace CidadeInteligente.Core.Services;

public interface IFileStorage
{
    Task DeleteFileAsync(string fileName, CancellationToken cancellationToken);
    Task<string> UploadOrUpdateFileAsync(string fileName, Stream stream, CancellationToken cancellationToken);
}
