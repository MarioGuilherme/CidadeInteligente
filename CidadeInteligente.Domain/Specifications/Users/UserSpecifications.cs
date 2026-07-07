using CidadeInteligente.Domain.Entities;

namespace CidadeInteligente.Domain.Specifications.Users;

public static class UserSpecifications
{
    public static SpecificationBuilder<User> GetById(int userId)
    {
        return SpecificationBuilder<User>.Create().Where(u => u.UserId == userId);
    }

    public static SpecificationBuilder<User> GetByCourseId(int courseId)
    {
        return SpecificationBuilder<User>.Create().Where(u => u.CourseId == courseId);
    }

    public static SpecificationBuilder<User> GetByToken(string token)
    {
        return SpecificationBuilder<User>.Create().Where(u => u.TokenRecoverPassword == token);
    }

    public static SpecificationBuilder<User> GetByEmailAndExceptUserId(string email, int? userIdExcept = default)
    {
        return SpecificationBuilder<User>.Create().Where(u => u.Email == email && u.UserId != userIdExcept);
    }
}
