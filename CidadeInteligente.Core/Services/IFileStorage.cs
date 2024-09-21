namespace CidadeInteligente.Core.Services;

public interface IFileStorage {
    Task DeleteFileAsync(string fileName);
    Task<string> UploadOrUpdateFileAsync(string fileName, byte[] bytes);
}