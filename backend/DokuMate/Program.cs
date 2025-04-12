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

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton<MongoDatabase, MongoDatabase>();
        builder.Services.AddSingleton<TagService, TagService>();
        builder.Services.AddSingleton<DocumentService, DocumentService>();
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        
        // TODO: Remove this
        DotEnv.Load("C:\\Entwicklung\\DocuMate\\dev.env");
        
        // TODO: PORT from .env File
        app.Run();
    }
}