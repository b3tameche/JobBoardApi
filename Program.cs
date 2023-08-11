
using Microsoft.EntityFrameworkCore;
using update.CustomMiddlewareConfig;
using update.CustomServiceConfig;
using update.Database;

namespace update;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // Custom services
        builder.Services.AddCustomServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
            app.UseSwagger();
            app.UseSwaggerUI(config => {
                config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
            });
        // }

        app.UseHttpsRedirection();

        // Custom middlewares
        app.UseCustomMiddlewares();

        app.MapControllers();

        using (var scope = app.Services.CreateScope()) {
            var provider = scope.ServiceProvider;

            var context = provider.GetRequiredService<ApplicationDbContext>();

            if (context.Database.GetMigrations().Any()) {
                context.Database.Migrate();
            }
        }

        app.Run();
    }
}
