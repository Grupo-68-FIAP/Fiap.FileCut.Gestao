
using Fiap.FileCut.Infra.Api;

namespace Fiap.FileCut.Gestao.Api;

public static class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        await builder.ConfigureFileCutGestaoApi();

        var app = builder.Build();

        await app.InitializeFileCutGestaoApi();
        app.MapControllers();

        var scope = app.Services.CreateScope();
        await scope.ScopedFileCutGestaoApi();

        await app.RunAsync();
    }
}
