
using Fiap.FileCut.Infra.Api.Configurations;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Fiap.FileCut.Gestao.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddJwtBearerAuthentication();
            builder.Services.AddSwaggerFC();
            builder.Services.AddEnvCors();
            builder.Services.AddNotifications()
                .EmailNotify(builder.Configuration);

            var app = builder.Build();

            app.UseSwaggerFC();
            app.UseEnvCors();
            app.UseHttpsRedirection();
            app.UseAuth();
            app.MapControllers();

            app.Run();
        }
    }
}
