namespace EmailMarketing.Models
{
    public class DisparoModel
    {
        public string Assunto { get; set; }
        public string CorpoEmail { get; set; }
        public int PromocaoId { get; set; }

        public IFormFile ImagemPromocao { get; set; }
    }
}
