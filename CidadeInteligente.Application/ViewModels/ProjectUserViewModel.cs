namespace CidadeInteligente.Application.ViewModels;

public class ProjectUserViewModel(long userId, string name) {
    public long UserId { get; private set; } = userId;
    public string Name { get; private set; } = name;
    public string MinorName => this.Name.Length > 58 ? this.Name[0..58] : this.Name;
}