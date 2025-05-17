using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InfoDengueAPI.Infrastructure.Data;
using InfoDengueAPI.Domain.Entities;

namespace InfoDengueAPI.WebAPI.Middlewares
{
    public class SolicitanteMiddleware
    {
        private readonly RequestDelegate _next;

        public SolicitanteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            var nome = context.Request.Headers["Nome"].ToString();
            var cpf = context.Request.Headers["CPF"].ToString();

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cpf))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Nome e CPF são obrigatórios.");
                return;
            }

            // Verificar se o CPF já existe
            var solicitante = await dbContext.Solicitantes.FirstOrDefaultAsync(s => s.CPF == cpf);

            if (solicitante == null)
            {
                // Criar novo solicitante
                solicitante = new Solicitante { Nome = nome, CPF = cpf };
                dbContext.Solicitantes.Add(solicitante);
                await dbContext.SaveChangesAsync();
            }

            // Armazenar o SolicitanteId no contexto
            context.Items["SolicitanteId"] = solicitante.Id;

            await _next(context);
        }
    }
}
