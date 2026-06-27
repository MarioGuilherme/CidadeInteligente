namespace CidadeInteligente.Application.Queries.GetUsers;

public record GetUsersQueryResult(IEnumerable<GetUsersQueryResult.UserViewModel> Users)
{
    public record UserViewModel(long UserId, string Name, string Email, string Course, long CourseId, string RoleDescription, byte Role)
    {
        public string MinorName => Name.Length > 58 ? Name[0..58] : Name;
    }
}
