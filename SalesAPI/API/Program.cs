
using Data;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            try
            {
                Log.Information("Starting...");
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddControllers();

                builder.Services.AddScoped<IEventPublisher, ConsoleEventPublisher>();

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryDb"));
                builder.Services.AddScoped<ISaleRepository, SaleRepository>();


                var app = builder.Build();

                // Configure the HTTP request pipeline.
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
