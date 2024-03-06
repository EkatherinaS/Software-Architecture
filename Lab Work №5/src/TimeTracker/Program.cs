using Tracker.TelegramBot.Middleware;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();
        app.UseMiddleware<OptionsMiddleware>();
        app.MapControllers();
        app.UseCors();

        app.MapControllerRoute(name: "default", pattern: "{controller}/{action}");
        app.Run();
    }
}