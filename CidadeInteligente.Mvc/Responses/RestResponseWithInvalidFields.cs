namespace CidadeInteligente.Mvc.Responses;

public record RestResponseWithInvalidFields
{
    public IReadOnlyDictionary<string, string[]> Notifications { get; init; } = new Dictionary<string, string[]>();
}
