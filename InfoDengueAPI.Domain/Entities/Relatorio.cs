using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoDengueAPI.Domain.Entities
{
    public class Relatorio
    {
        public int Id { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string Arbovirose { get; set; } = string.Empty;
        public int SemanaInicio { get; set; }
        public int SemanaFim { get; set; }
        public int CodigoIBGE { get; set; }
        public string Municipio { get; set; } = string.Empty;

        public int SolicitanteId { get; set; }
        public Solicitante Solicitante { get; set; }

        public ICollection<DadoEpidemiologico> DadosEpidemiologicos { get; set; } = new List<DadoEpidemiologico>();
    }
}
