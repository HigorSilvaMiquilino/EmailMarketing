namespace EmailMarketing.Data
{
    public class Promocao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>(); 
    }
}
