namespace EmailMarketing.Data
{
    public class LogEnvio
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public DateTime DataEnvio { get; set; }
        public int PromocaoId { get; set; }
        public string Status { get; set; }

        public Promocao Promocao { get; set; }

    }
}
