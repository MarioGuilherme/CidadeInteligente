using System.ComponentModel;

namespace CidadeInteligente.Core.Enums;

public enum Role : byte
{
    [Description("Professor")] Teacher,
    [Description("Aluno")] Student
}
