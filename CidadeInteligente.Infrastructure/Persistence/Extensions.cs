using CidadeInteligente.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence;

public static class Extensions {
    public static async Task<PaginationResult<T>> GetPaged<T>(
        this IQueryable<T> query,
        int currentPage
    ) where T : class {
        PaginationResult<T> result = new() {
            CurrentPage = currentPage,
            ItemsCount = await query.CountAsync()
        };

        double pageCount = (double)result.ItemsCount / result.PageSize;
        result.TotalPages = (int)Math.Ceiling(pageCount);

        int skip = (currentPage - 1) * result.PageSize;

        result.Data = await query.Skip(skip).Take(result.PageSize).ToListAsync();

        return result;
    }

    public static PaginationResult<T> GetPaged<T>(
        this IEnumerable<T> query,
        int currentPage
    ) where T : class {
        PaginationResult<T> result = new() {
            CurrentPage = currentPage,
            ItemsCount = query.Count()
        };

        double pageCount = (double)result.ItemsCount / result.PageSize;
        result.TotalPages = (int)Math.Ceiling(pageCount);

        int skip = (currentPage - 1) * result.PageSize;

        result.Data = query.Skip(skip).Take(result.PageSize).ToList();

        return result;
    }
}