namespace CidadeInteligente.Core.Models;

public class PaginationResult<T>
{
    public int PageSize = 8;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int ItemsCount { get; set; }
    public List<T> Data { get; set; } = null!;

    public PaginationResult() { }

    public PaginationResult(int page, int totalPages, int itemsCount, List<T> data)
    {
        CurrentPage = page;
        TotalPages = totalPages;
        ItemsCount = itemsCount;
        Data = data;
    }
}