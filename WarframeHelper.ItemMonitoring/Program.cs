using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintAnge.WarframeHelper.Data;
using Telegram.Bot;

namespace MintAnge.WarframeHelper.ItemMonitoring;

internal class Program
{
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        PostgresConfiguration postgresConf = builder.Configuration.GetRequiredSection("PostgresConfig").Get<PostgresConfiguration>();
        var contextOptions = new DbContextOptionsBuilder<WarframeMarketContext>()
                                                        .UseNpgsql(postgresConf.GetConnectionString())
                                                        .Options;



        builder.Services.AddHostedService<BackgroundItemMonitoringService>();
        builder.Services.AddSingleton(new TelegramBotClient(builder.Configuration["BotToken"]));
        builder.Services.AddSingleton<WarframeMarketAPI.WarframeMarketAPI>();
        builder.Services.AddDbContext<WarframeMarketContext>(
                options => options.UseNpgsql(postgresConf.GetConnectionString())
        );

        Console.WriteLine(builder.Environment.EnvironmentName);
        Console.WriteLine(builder.Configuration.AsEnumerable().ToList());

        IHost host = builder.Build();
        host.Run();
    }
}