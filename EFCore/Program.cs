using Microsoft.EntityFrameworkCore;
using EFCore.Models;
using EFCore.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

//Tarefas pendentes:
// X-Criar uma rota de autentica��o, para verificar se os dados de usuario recebidos batem com todos os dados de um determinado usuario no banco
// X-Fazer com o que o programa reconhe�a automaticamente em qual computador est� sendo executado, de casa ou do trabalho, para alterar o nome do host na connection string do banco de dados
// X-(02/08/23)Algoritmo para registrar todas as movimenta�oes da conta (em falta: deposito e saque)

namespace EFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<SistemaBancoContext>();
            builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); //Permite a utilizacao de campos relacionais entre tabelas(navigation), sem ocasionar loop infinito
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IContaRepository, ContaRepository>();
            builder.Services.AddScoped<IMovRepository, MovRepository>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });


            //Configurando o CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}