namespace CidadeInteligente.Application.ViewModels;

public class PaginatedView<T> {
    public ICollection<T> Data { get; set; } = [];
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public const int ItensPerPage = 8;
}