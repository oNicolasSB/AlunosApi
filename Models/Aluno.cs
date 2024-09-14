using System.ComponentModel.DataAnnotations;

namespace AlunosApi.Models;

public class Aluno
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(80, ErrorMessage = "O nome do aluno deve conter no máximo 80 caracteres")]
    public string Nome { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(100, ErrorMessage = "O email do aluno deve conter no máximo 100 caracteres")]
    public string Email { get; set; } = string.Empty;
    public int Idade { get; set; }
}
