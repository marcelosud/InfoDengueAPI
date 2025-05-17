using System.Collections.Generic;

namespace InfoDengueAPI.Domain.Entities
{
    public class Solicitante
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;

        public ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();
    }
}
