using System.ComponentModel.DataAnnotations;


namespace EmailMarketing.Models
{
    public class RegistrarUserModel
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MinLength(2, ErrorMessage = "Nome deve ser de pelo menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "Não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor entre com um e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório.")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        [MaxLength(100, ErrorMessage = "Não pode passar de 100 caracteres.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}

