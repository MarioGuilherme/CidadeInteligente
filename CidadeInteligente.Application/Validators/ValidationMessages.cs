using CidadeInteligente.Domain.Constants;

namespace CidadeInteligente.Application.Validators;

public static class ValidationMessages
{
    public static class Common
    {
        public const string InvalidPage = "A página é inválida";
    }

    public static class Area
    {
        public const string InvalidId = "O identificador da área é inválido";
        public const string DescriptionRequired = "A descrição da área é obrigatória";
        public static readonly string DescriptionMaxLength = $"A descrição da área não pode exceder {AreaConstraints.DescriptionMaxLength} caracteres";
    }

    public static class Course
    {
        public const string InvalidId = "O identificador do curso é inválido";
        public const string DescriptionRequired = "A descrição do curso é obrigatória";
        public static readonly string DescriptionMaxLength = $"A descrição do curso não pode exceder {CourseConstraints.DescriptionMaxLength} caracteres";
    }

    public static class User
    {
        public const string InvalidId = "O identificador do usuário é inválido";
        public const string CourseRequired = "É necessário especificar o curso do usuário";
        public const string NameRequired = "O nome do usuário é obrigatório";
        public static readonly string NameMaxLength = $"O nome do usuário não pode exceder {UserConstraints.NameMaxLength} caracteres";
        public const string EmailRequired = "O e-mail do usuário é obrigatório";
        public const string EmailInvalid = "O e-mail do usuário não é válido";
        public static readonly string EmailMaxLength = $"O e-mail do usuário não pode exceder {UserConstraints.EmailMaxLength} caracteres";
        public const string PasswordRequired = "A nova senha é obrigatória";
        public static readonly string PasswordMinLength = $"A senha deve ter pelo menos {UserConstraints.RawPasswordMinLength} caracteres";
        public const string ConfirmPasswordRequired = "A confirmação da senha é obrigatória";
        public const string PasswordsDoNotMatch = "As senhas não conferem";
        public const string TokenRequired = "O token de redefinição de senha é obrigatório";
        public static readonly string TokenLength = $"O token de redefinição de senha deve ter {UserConstraints.TokenRecoverPasswordMaxLength} caracteres";
        public const string RoleRequired = "É necessário especificar uma permissão para o usuário!";
    }

    public static class Media
    {
        public const string InvalidId = "O identificador da mídia é inválido";
        public const string TitleRequired = "O título da mídia é obrigatório";
        public static readonly string TitleMaxLength = $"O título da mídia não pode exceder {MediaConstraints.TitleMaxLength} caracteres";
        public const string MimeTypeRequired = "O tipo (mime type) da mídia é obrigatório";
        public static readonly string MimeTypeNotSupported = $"Tipo de mídia não suportado! Tipos aceitos: {string.Join(", ", MediaConstraints.AllowedMimeTypes)}";
        public static readonly string FileMaxSize = $"O arquivo da mídia não pode exceder {MediaConstraints.FileMaxSizeInMegaBytes} MB";
        public static readonly string DescriptionMaxLength = $"A descrição da mídia não pode exceder {MediaConstraints.DescriptionMaxLength} caracteres";
    }

    public static class Project
    {
        public const string InvalidId = "O identificador do projeto é inválido";
        public const string CreatorRequired = "É necessário especificar o criador do projeto";
        public const string AreaRequired = "É necessário especificar a área do projeto";
        public const string CourseRequired = "É necessário especificar o curso do projeto";
        public const string TitleRequired = "O título do projeto é obrigatório";
        public static readonly string TitleMaxLength = $"O título do projeto não pode exceder {ProjectConstraints.TitleMaxLength} caracteres";
        public static readonly string DescriptionMaxLength = $"A descrição do projeto não pode exceder {ProjectConstraints.DescriptionMaxLength} caracteres";
        public const string StartedAtRequired = "A data de início do projeto é obrigatória";
        public const string StartedAtInFuture = "A data de início do projeto não pode estar no futuro";
        public const string FinishedAtBeforeStartedAt = "A data de término do projeto não pode ser anterior à data de início";
        public const string FinishedAtInFuture = "A data de término do projeto não pode estar no futuro";
        public const string InvolvedUsersRequired = "O projeto deve ter pelo menos um usuário envolvido";
        public const string InvalidInvolvedUser = "Informe um usuário válido";
        public const string MediasRequired = "O projeto deve ter pelo menos uma mídia anexada";
    }
}
