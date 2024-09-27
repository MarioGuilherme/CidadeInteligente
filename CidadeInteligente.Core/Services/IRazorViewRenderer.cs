namespace CidadeInteligente.Core.Services;

public interface IRazorViewRenderer {
    Task<string> RenderViewToStringAsync<T>(string viewName, T model, string appUrl);
}