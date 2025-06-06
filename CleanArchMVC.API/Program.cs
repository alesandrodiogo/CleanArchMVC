using CleanArchMVC.Infra.IoC;
using Microsoft.OpenApi.Models;

namespace CleanArchMVC.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddInfrastructureAPI(builder.Configuration);

        //Ativar a autenticacao e validar o token
        builder.Services.AddInfrastructureJWT(builder.Configuration);
        builder.Services.AddInfrastructureSwagger();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStatusCodePages();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
