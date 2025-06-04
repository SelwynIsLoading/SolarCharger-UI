using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using raspi.Components;
using raspi.Services;
using raspi.Services.Models;

namespace raspi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddMudServices();

        builder.Services.AddControllers();

        // builder.Services.AddSingleton<CoinSlotService>();
        
        builder.Services.AddSingleton<CountdownManager>();
        builder.Services.AddScoped<CountdownService>();
        builder.Services.AddScoped<DbService>();
  
        builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddHttpClient<FingerprintService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8000");
        });
        
        builder.Services.AddHttpClient<CoinSlotService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8000");
        });

        builder.Services.AddHttpClient<ArduinoService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8000");
        });

        builder.Services.AddSignalR();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.MapControllers();
        app.MapHub<CoinHub>("/coinhub");
        app.MapHub<FingerprintHub>("/fingerprinthub");

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
