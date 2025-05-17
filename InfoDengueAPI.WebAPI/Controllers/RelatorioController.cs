using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfoDengueAPI.Infrastructure.Data;
using InfoDengueAPI.Domain.Entities;
using InfoDengueAPI.Application.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace InfoDengueAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IInfodengueService _infodengueService;

        public RelatorioController(AppDbContext context, IInfodengueService infodengueService)
        {
            _context = context;
            _infodengueService = infodengueService;
        }

        [HttpPost("consulta-externa")]
        public async Task<IActionResult> ConsultarDadosExternos(
            [FromQuery] int codigoIBGE,
            [FromQuery] string disease)
        {
            if (!HttpContext.Items.ContainsKey("SolicitanteId"))
                return BadRequest(new { message = "Solicitante não identificado." });

            int solicitanteId = (int)HttpContext.Items["SolicitanteId"];
            int ewStart = 1;
            int ewEnd = 1;
            int eyStart = 2025;
            int eyEnd = 2025;

            try
            {
                var dados = await _infodengueService.GetEpidemiologicalData(codigoIBGE, ewStart, ewEnd, eyStart, eyEnd, disease);

                if (dados == null || dados.Count == 0)
                {
                    return NotFound(new { message = "Nenhum dado encontrado para os parâmetros fornecidos." });
                }

                // Definir o município com base no código IBGE
                string municipio = codigoIBGE switch
                {
                    3304557 => "Rio de Janeiro",
                    4106902 => "Curitiba",
                    3550308 => "São Paulo",
                    _ => "Desconhecido"
                };

                // Criar um novo relatório
                var relatorio = new Relatorio
                {
                    DataSolicitacao = DateTime.Now,
                    CodigoIBGE = codigoIBGE,
                    SemanaInicio = ewStart,
                    SemanaFim = ewEnd,
                    Arbovirose = disease,
                    Municipio = municipio,
                    SolicitanteId = solicitanteId
                };

                _context.Relatorios.Add(relatorio);
                await _context.SaveChangesAsync();

                // Processar e salvar os dados retornados
                foreach (var item in dados)
                {
                    var dado = new DadoEpidemiologico
                    {
                        DataInicioSE = item["data_iniSE"]?.Value<long>() ?? 0,
                        SE = item["SE"]?.Value<int>() ?? 0,
                        CasosEst = item["casos_est"]?.Value<double>() ?? 0,
                        CasosEstMin = item["casos_est_min"]?.Value<int>() ?? 0,
                        CasosEstMax = item["casos_est_max"]?.Value<int>() ?? 0,
                        Casos = item["casos"]?.Value<int>() ?? 0,
                        Prt1 = item["p_rt1"]?.Value<double>() ?? 0,
                        Pinc100k = item["p_inc100k"]?.Value<double>() ?? 0,
                        LocalidadeId = item["Localidade_id"]?.Value<int>() ?? 0,
                        Nivel = item["nivel"]?.Value<int>() ?? 0,
                        VersaoModelo = item["versao_modelo"]?.ToString() ?? string.Empty,
                        Rt = item["Rt"]?.Value<double>(),
                        Populacao = item["pop"]?.Value<double>() ?? 0,
                        TempMin = item["tempmin"]?.Value<double>() ?? 0,
                        UmidMax = item["umidmax"]?.Value<double>() ?? 0,
                        Receptivo = item["receptivo"]?.Value<int>() ?? 0,
                        Transmissao = item["transmissao"]?.Value<int>() ?? 0,
                        NivelInc = item["nivel_inc"]?.Value<int>() ?? 0,
                        UmidMed = item["umidmed"]?.Value<double>() ?? 0,
                        UmidMin = item["umidmin"]?.Value<double>() ?? 0,
                        TempMed = item["tempmed"]?.Value<double>() ?? 0,
                        TempMax = item["tempmax"]?.Value<double>() ?? 0,
                        CasosProv = item["casprov"]?.Value<int>() ?? 0,
                        NotifAccumYear = item["notif_accum_year"]?.Value<int>() ?? 0,
                        RelatorioId = relatorio.Id
                    };

                    _context.DadosEpidemiologicos.Add(dado);
                }

                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Dados epidemiológicos salvos com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }



        /// <summary>
        /// Lista todos os relatórios salvos no banco.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var relatorios = await _context.Relatorios
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .ToListAsync();

            var result = relatorios.Select(r => new
            {
                r.Id,
                r.DataSolicitacao,
                r.Arbovirose,
                r.SemanaInicio,
                r.SemanaFim,
                r.CodigoIBGE,
                r.Municipio,
                Solicitante = new
                {
                    r.Solicitante.Id,
                    r.Solicitante.Nome,
                    r.Solicitante.CPF
                },
                DadosEpidemiologicos = r.DadosEpidemiologicos.Select(d => new
                {
                    d.Id,
                    d.SE,
                    d.CasosEst,
                    d.Casos,
                    d.Pinc100k,
                    d.Rt,
                    d.Populacao,
                    d.TempMin,
                    d.TempMax,
                    d.UmidMin,
                    d.UmidMax,
                    d.NotifAccumYear
                })
            });

            return Ok(result);
        }


        /// <summary>
        /// Consulta relatórios por Código IBGE.
        /// </summary>
        [HttpGet("by-ibge/{codigoIBGE}")]
        public async Task<IActionResult> GetByIBGE(int codigoIBGE)
        {
            var relatorios = await _context.Relatorios
                .Where(r => r.CodigoIBGE == codigoIBGE)
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .ToListAsync();

            if (!relatorios.Any())
                return NotFound(new { message = "Nenhum relatório encontrado para o código IBGE informado." });

            return Ok(relatorios);
        }

        /// <summary>
        /// Consulta relatórios por arbovirose.
        /// </summary>
        [HttpGet("by-arbovirose/{arbovirose}")]
        public async Task<IActionResult> GetByArbovirose(string arbovirose)
        {
            var relatorios = await _context.Relatorios
                .Where(r => r.Arbovirose.ToLower() == arbovirose.ToLower())
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .ToListAsync();

            if (!relatorios.Any())
                return NotFound(new { message = "Nenhum relatório encontrado para a arbovirose informada." });

            return Ok(relatorios);
        }

        /// <summary>
        /// Consulta relatórios por semana início e semana fim.
        /// </summary>
        [HttpGet("by-semana")]
        public async Task<IActionResult> GetBySemana([FromQuery] int semanaInicio, [FromQuery] int semanaFim)
        {
            var relatorios = await _context.Relatorios
                .Where(r => r.SemanaInicio >= semanaInicio && r.SemanaFim <= semanaFim)
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .ToListAsync();

            if (!relatorios.Any())
                return NotFound(new { message = "Nenhum relatório encontrado para o período informado." });

            return Ok(relatorios);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Relatorio relatorio)
        {
            if (!HttpContext.Items.ContainsKey("SolicitanteId"))
                return BadRequest(new { message = "Solicitante não identificado." });

            int solicitanteId = (int)HttpContext.Items["SolicitanteId"];

            relatorio.SolicitanteId = solicitanteId;
            relatorio.DataSolicitacao = DateTime.Now;

            _context.Relatorios.Add(relatorio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = relatorio.Id }, relatorio);
        }


        [HttpGet("by-solicitante/{solicitanteId}")]
        public async Task<IActionResult> GetBySolicitante(int solicitanteId)
        {
            var relatorios = await _context.Relatorios
                .Where(r => r.SolicitanteId == solicitanteId)
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .ToListAsync();

            if (!relatorios.Any())
                return NotFound(new { message = "Nenhum relatório encontrado para o solicitante informado." });

            return Ok(relatorios);
        }

        [HttpGet("by-data")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var relatorios = await _context.Relatorios
                .Where(r => r.DataSolicitacao >= startDate && r.DataSolicitacao <= endDate)
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .ToListAsync();

            if (!relatorios.Any())
                return NotFound(new { message = "Nenhum relatório encontrado para o período informado." });

            return Ok(relatorios);
        }

        [HttpGet("casos-por-arbovirose")]
        public async Task<IActionResult> GetCasesByArbovirose()
        {
            var casos = await _context.Relatorios
                .GroupBy(r => r.Arbovirose)
                .Select(g => new
                {
                    Arbovirose = g.Key,
                    TotalCasos = g.Count()
                })
                .ToListAsync();

            return Ok(casos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var relatorio = await _context.Relatorios
                .Include(r => r.Solicitante)
                .Include(r => r.DadosEpidemiologicos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (relatorio == null)
            {
                return NotFound(new { message = "Relatório não encontrado." });
            }

            var result = new
            {
                relatorio.Id,
                relatorio.DataSolicitacao,
                relatorio.Arbovirose,
                relatorio.SemanaInicio,
                relatorio.SemanaFim,
                relatorio.CodigoIBGE,
                relatorio.Municipio,
                Solicitante = new
                {
                    relatorio.Solicitante.Id,
                    relatorio.Solicitante.Nome,
                    relatorio.Solicitante.CPF
                },
                DadosEpidemiologicos = relatorio.DadosEpidemiologicos.Select(d => new
                {
                    d.Id,
                    d.SE,
                    d.CasosEst,
                    d.Casos,
                    d.Pinc100k,
                    d.Rt,
                    d.Populacao,
                    d.TempMin,
                    d.TempMax,
                    d.UmidMin,
                    d.UmidMax,
                    d.NotifAccumYear
                })
            };

            return Ok(result);
        }

        [HttpGet("dados-epidemiologicos-rj-sp")]
        public async Task<IActionResult> GetDadosEpidemiologicosRJSP()
        {
            var municipios = new[] { "Rio de Janeiro", "São Paulo" };

            var dados = await _context.DadosEpidemiologicos
                .Include(d => d.Relatorio)
                .Where(d => municipios.Contains(d.Relatorio.Municipio))
                .Select(d => new
                {
                    d.Id,
                    d.SE,
                    d.CasosEst,
                    d.Casos,
                    d.Pinc100k,
                    d.Rt,
                    d.Populacao,
                    d.TempMin,
                    d.TempMax,
                    d.UmidMin,
                    d.UmidMax,
                    d.NotifAccumYear,
                    Municipio = d.Relatorio.Municipio,
                    DataSolicitacao = d.Relatorio.DataSolicitacao,
                    Arbovirose = d.Relatorio.Arbovirose
                })
                .ToListAsync();

            if (!dados.Any())
                return NotFound(new { message = "Nenhum dado epidemiológico encontrado para Rio de Janeiro e São Paulo." });

            return Ok(dados);
        }

        [HttpGet("total-casos-rj-sp")]
        public async Task<IActionResult> GetTotalCasosRJSP()
        {
            var municipios = new[] { "Rio de Janeiro", "São Paulo" };

            var totalCasos = await _context.DadosEpidemiologicos
                .Include(d => d.Relatorio)
                .Where(d => municipios.Contains(d.Relatorio.Municipio))
                .GroupBy(d => d.Relatorio.Municipio)
                .Select(g => new
                {
                    Municipio = g.Key,
                    TotalCasos = g.Sum(d => d.Casos)
                })
                .ToListAsync();

            if (!totalCasos.Any())
                return NotFound(new { message = "Nenhum caso epidemiológico encontrado para Rio de Janeiro e São Paulo." });

            return Ok(totalCasos);
        }




    }
}
