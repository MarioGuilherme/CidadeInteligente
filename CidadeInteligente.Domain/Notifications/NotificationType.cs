using System.ComponentModel;

namespace CidadeInteligente.Domain.Notifications;

public enum NotificationType : byte
{
    [Description("O e-mail e/ou a senha não conferem.")] InvalidLoginCredentials,
    [Description("O usuário com ID {0} não foi encontrado.")] UserNotFound,
    [Description("A área com ID {0} não foi encontrada.")] AreaNotFound,
    [Description("A área com descrição '{0}' já existe.")] AreaAlreadyExists,
    [Description("O curso com descrição '{0}' já existe.")] CourseAlreadyExists,
    [Description("O curso com ID {0} não foi encontrado.")] CourseNotFound,
    [Description("O projeto com ID {0} não foi encontrado.")] ProjectNotFound,
    [Description("A mídia com ID {0} não foi encontrada.")] MediaNotFound,
    [Description("O usuário com ID {0} não tem autorização para modificar o projeto com ID {1}.")] UserNotAuthorizedToModifyProject,
    [Description("A área com ID {0} possui projetos dependentes e não pode ser excluída.")] AreaWithDependentProjects,
    [Description("O curso com ID {0} possui projetos ou usuários dependentes e não pode ser excluído.")] CourseWithDependentProjectsOrUser,
    [Description("O usuário com ID {0} possui projetos dependentes e não pode ser excluído.")] UserWithDependentProjects,
    [Description("O usuário com o token informado não foi encontrado.")] UserWithTokenNotFound,
    [Description("O usuário com o e-mail informado não foi encontrado.")] UserWithEmailNotFound,
    [Description("O token de recuperação de senha expirou.")] TokenRecoverPasswordExpired,
    [Description("O e-mail informado já está em uso.")] EmailAlreadyInUse
}
