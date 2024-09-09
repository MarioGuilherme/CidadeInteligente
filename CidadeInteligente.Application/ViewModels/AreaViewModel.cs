namespace CidadeInteligente.Application.ViewModels;

public class AreaViewModel(long areaId, string description) {
    public long AreaId { get; private set; } = areaId;
    public string Description { get; private set; } = description;
}