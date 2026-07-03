using System.ComponentModel;

namespace CidadeInteligente.Domain.Enums;

public enum Role : byte
{
    [Description("Professor")] Teacher,
    [Description("Aluno")] Student
}
