using System.ComponentModel;

namespace CidadeInteligente.Core.Notifications;

public enum NotificationType : byte
{
    [Description("The email and/or password do not match.")] InvalidLoginCredentials,
    [Description("The user with ID {0} was not found.")] UserNotFound,
    [Description("The area with ID {0} was not found.")] AreaNotFound,
    [Description("The course with ID {0} was not found.")] CourseNotFound,
    [Description("The project with ID {0} was not found.")] ProjectNotFound,
    [Description("The media with ID {0} was not found.")] MediaNotFound,
    [Description("User with ID {0} is not authorized to modify project with ID {1}.")] UserNotAuthorizedToModifyProject,
    [Description("The area with ID {0} has dependent projects and cannot be deleted.")] AreaWithDependentProjects,
    [Description("The course with ID {0} has dependent projects and cannot be deleted.")] CourseWithDependentProjects,
    [Description("The user with ID {0} has dependent projects and cannot be deleted.")] UserWithDependentProjects,
    [Description("The user with specific Token was not found.")] UserWithTokenNotFound,
    [Description("The user with specific email was not found.")] UserWithEmailNotFound,
    [Description("The token for password recovery has expired.")] TokenRecoverPasswordExpired,
    [Description("The supplied email address is already in use.")] EmailAlreadyInUse
}
