namespace CidadeInteligente.UI.ViewModels;

public class ErrorViewModel(int statusCode, string title = "Erro desconhecido", string? description = null) {
    public int StatusCode { get; private set; } = statusCode;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description ?? title;
}