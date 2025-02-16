using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Data
{
    public class EmailLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime DataEnvio { get; set; }

        [Required]
        public string MensagemErro { get; set; }

    }
}
