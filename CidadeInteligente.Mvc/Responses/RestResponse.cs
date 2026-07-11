namespace CidadeInteligente.Mvc.Responses;

public record RestResponse(object? Data = default)
{
    public IEnumerable<string>? Notifications { get; init; } = default;
}

public record RestResponse<TNotification>(object? Data = default)
{
    public TNotification? Notifications { get; init; } = default;
}
