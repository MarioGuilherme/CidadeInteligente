using System.ComponentModel;

namespace CidadeInteligente.Core.Enums;

public enum Role : byte {
    [Description("Professor")] Teacher = 1,
    [Description("Aluno")] Student
}