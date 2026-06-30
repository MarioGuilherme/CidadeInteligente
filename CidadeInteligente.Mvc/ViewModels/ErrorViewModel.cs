namespace CidadeInteligente.Mvc.ViewModels;

public record ErrorViewModel(int StatusCode, string Title = "Erro desconhecido", string? Description = null)
{
    public string Description { get; private set; } = Description ?? Title;
}
