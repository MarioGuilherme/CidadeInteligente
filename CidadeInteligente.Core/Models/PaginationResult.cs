namespace CidadeInteligente.Core.Models;

public record PaginationResult<T>
{
    public int PageSize = 8;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int ItemsCount { get; set; }
    public IEnumerable<T> Data { get; set; } = null!;

    public PaginationResult() { }

    public PaginationResult(int page, int totalPages, int itemsCount, IEnumerable<T> data)
    {
        CurrentPage = page;
        TotalPages = totalPages;
        ItemsCount = itemsCount;
        Data = data;
    }
}
