using System;

namespace InfoDengueAPI.Domain.Entities
{
    public class DadoEpidemiologico
    {
        public int Id { get; set; }
        public long DataInicioSE { get; set; }
        public int SE { get; set; }
        public double CasosEst { get; set; }
        public int CasosEstMin { get; set; }
        public int CasosEstMax { get; set; }
        public int Casos { get; set; }
        public double Prt1 { get; set; }
        public double Pinc100k { get; set; }
        public int LocalidadeId { get; set; }
        public int Nivel { get; set; }
        public string VersaoModelo { get; set; } = string.Empty;
        public double? Rt { get; set; }
        public double Populacao { get; set; }
        public double TempMin { get; set; }
        public double UmidMax { get; set; }
        public int Receptivo { get; set; }
        public int Transmissao { get; set; }
        public int NivelInc { get; set; }
        public double UmidMed { get; set; }
        public double UmidMin { get; set; }
        public double TempMed { get; set; }
        public double TempMax { get; set; }
        public int CasosProv { get; set; }
        public int? CasosProvEst { get; set; }
        public int? CasosProvEstMin { get; set; }
        public int? CasosProvEstMax { get; set; }
        public int? CasosConf { get; set; }
        public int NotifAccumYear { get; set; }

        public int RelatorioId { get; set; }
        public Relatorio Relatorio { get; set; }
    }
}
