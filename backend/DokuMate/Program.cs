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
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        
        // TODO: Remove this
        DotEnv.Load("C:\\Entwicklung\\DocuMate\\dev.env");

        app.Run();
    }
}