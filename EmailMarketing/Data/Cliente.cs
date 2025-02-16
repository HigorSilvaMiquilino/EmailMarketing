﻿namespace EmailMarketing.Data
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int? PromocaoId { get; set; } // Chave estrangeira
        public Promocao Promocao { get; set; } // Navegação
    }
}
