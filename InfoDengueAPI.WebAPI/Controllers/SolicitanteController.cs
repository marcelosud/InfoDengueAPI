using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfoDengueAPI.Infrastructure.Data;
using InfoDengueAPI.Domain.Entities;
using System.Threading.Tasks;
using InfoDengueAPI.WebAPI.DTOs;

namespace InfoDengueAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitanteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitanteController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os solicitantes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var solicitantes = await _context.Solicitantes.ToListAsync();
            return Ok(solicitantes);
        }

        /// <summary>
        /// Cria um novo solicitante.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SolicitanteRequest request)
        {
            if (string.IsNullOrEmpty(request.Nome) || string.IsNullOrEmpty(request.CPF))
            {
                return BadRequest(new { message = "Nome e CPF são obrigatórios." });
            }

            var existingSolicitante = await _context.Solicitantes
                .FirstOrDefaultAsync(s => s.CPF == request.CPF);

            if (existingSolicitante != null)
            {
                return Conflict(new { message = "Solicitante já cadastrado." });
            }

            var solicitante = new Solicitante
            {
                Nome = request.Nome,
                CPF = request.CPF
            };

            _context.Solicitantes.Add(solicitante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = solicitante.Id }, solicitante);
        }



    }
}
