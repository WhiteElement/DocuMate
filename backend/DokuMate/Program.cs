using DokuMate.Database;
using DokuMate.Document;
using DokuMate.Helpers;
using DokuMate.Tag;

namespace DokuMate;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DotEnv.Load(args);
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton<MongoDatabase, MongoDatabase>();
        builder.Services.AddSingleton<TagService, TagService>();
        builder.Services.AddSingleton<DocumentService, DocumentService>();
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        if (DotEnv.TryGetVar("PORT", out string value))
            builder.WebHost.UseUrls($"http://localhost:{value}");

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
        
        // TODO: README
    }
}