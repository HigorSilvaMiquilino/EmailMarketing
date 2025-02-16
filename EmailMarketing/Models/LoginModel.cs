using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
