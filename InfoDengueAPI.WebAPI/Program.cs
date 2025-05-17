using InfoDengueAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using InfoDengueAPI.Application.Services;
using System.Text.Json.Serialization;
using InfoDengueAPI.WebAPI.Middlewares;
using InfoDengueAPI.WebAPI.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InfoDengueAPI", Version = "v1" });

    // Adicionar headers globais
    c.AddSecurityDefinition("Nome", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Nome",
        Type = SecuritySchemeType.ApiKey,
        Description = "Informe o nome do usuário solicitante"
    });

    c.AddSecurityDefinition("CPF", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "CPF",
        Type = SecuritySchemeType.ApiKey,
        Description = "Informe o CPF do usuário solicitante"
    });

    c.OperationFilter<AddRequiredHeaders>();
});


// Registro do HttpClient e do Serviço InfodengueService
builder.Services.AddHttpClient<IInfodengueService, InfodengueService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InfoDengueAPI v1");
    });
}
app.UseMiddleware<SolicitanteMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
