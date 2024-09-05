namespace CidadeInteligente.Core.Services;

public interface IFileStorage {
    Task DeleteFileAsync(string fileName);
    Task<string> UploadFileAsync(string extension, byte[] bytes);
}