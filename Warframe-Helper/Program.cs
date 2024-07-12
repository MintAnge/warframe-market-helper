using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using System.IO;
using MintAnge.WarframeMarketHelper;
using MintAnge.WarframeMarketApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MintAnge.WarframeMarketApi;
using Microsoft.Extensions.Logging;
using Warframe_Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Warframe_Helper.Data;

//TelegramBotClient _botClient = new TelegramBotClient(File.ReadAllText("C:\\Users\\AngeMaks\\source\\repos\\MintAnge\\warframe-market-helper\\bot_token.txt"));

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<BackgroundBotService>();
Console.WriteLine(builder.Environment.EnvironmentName);

PostgresConfiguration postgresConf = builder.Configuration.GetRequiredSection("PostgresConfig").Get<PostgresConfiguration>();

Console.WriteLine(builder.Configuration.AsEnumerable().ToList());

builder.Services.AddSingleton<TelegramBotClient>(new TelegramBotClient(builder.Configuration["BotToken"]));
builder.Services.AddSingleton<WarframeMarketAPI>();
builder.Services.AddSingleton<UpdErrHandlers>();
builder.Services.AddDbContext<WarframeMarketContext>(
        options => options.UseNpgsql(postgresConf.GetConnectionString())
);

IHost host = builder.Build();
host.Run();
