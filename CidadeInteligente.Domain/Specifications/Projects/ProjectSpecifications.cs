using CidadeInteligente.Domain.Entities;

namespace CidadeInteligente.Domain.Specifications.Projects;

public static class ProjectSpecifications
{
    public static SpecificationBuilder<Project> GetById(int projectId)
    {
        return SpecificationBuilder<Project>.Create().Where(p => p.ProjectId == projectId);
    }

    public static SpecificationBuilder<Project> GetByAreaId(int areaId)
    {
        return SpecificationBuilder<Project>.Create().Where(p => p.AreaId == areaId);
    }

    public static SpecificationBuilder<Project> GetByCourseId(int courseId)
    {
        return SpecificationBuilder<Project>.Create().Where(p => p.CourseId == courseId);
    }

    public static SpecificationBuilder<Project> GetRelatedProjectsFromUser(int userId)
    {
        return SpecificationBuilder<Project>.Create().Where(p => p.CreatedByUserId == userId || p.InvolvedUsers.Any(iu => iu.UserId == userId));
    }
}
