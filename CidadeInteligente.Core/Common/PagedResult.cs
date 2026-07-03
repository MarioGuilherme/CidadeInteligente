namespace CidadeInteligente.Core.Common;

//public record PagedResult<T>
//{
//    public int PageSize = 8;
//    public int CurrentPage { get; set; }
//    public int TotalPages { get; set; }
//    public int ItemsCount { get; set; }
//    public IEnumerable<T> Data { get; set; } = null!;

//    public PagedResult() { }

//    public PagedResult(int page, int totalPages, int itemsCount, IEnumerable<T> data)
//    {
//        CurrentPage = page;
//        TotalPages = totalPages;
//        ItemsCount = itemsCount;
//        Data = data;
//    }
//}
public record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public PagedResult(IReadOnlyList<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        Page = pageNumber;
        PageSize = pageSize;
        TotalPages = pageSize > 0
            ? (int)Math.Ceiling(totalItems / (double)pageSize)
            : 0;
    }
}
