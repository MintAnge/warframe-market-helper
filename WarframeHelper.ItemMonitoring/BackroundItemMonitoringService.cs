using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using MintAnge.WarframeHelper.Data;
using MintAnge.WarframeMarketAPI.Models;
using MintAnge.WarframeMarketAPI;
using Item = MintAnge.WarframeHelper.Data.Models.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MintAnge.WarframeHelper.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace MintAnge.WarframeHelper.ItemMonitoring;

public class BackgroundItemMonitoringService : BackgroundService
{
    private readonly ILogger<BackgroundItemMonitoringService> _logger;
    private readonly TelegramBotClient _botClient;
    private readonly WarframeMarketAPI.WarframeMarketAPI _warframeMarketAPI;
    private readonly IServiceScopeFactory serviceScopeFactory;
    public BackgroundItemMonitoringService(TelegramBotClient botClient,
                                           ILogger<BackgroundItemMonitoringService> logger,
                                           IServiceScopeFactory serviceScopeFactory,
                                           WarframeMarketAPI.WarframeMarketAPI warframeMarketAPI)
    {
        _botClient = botClient;
        _logger = logger;
        _warframeMarketAPI = warframeMarketAPI;
        this.serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //_botClient = new TelegramBotClient(File.ReadAllText("C:\\Users\\AngeMaks\\source\\repos\\MintAnge\\warframe-market-helper\\bot_token.txt"));
        using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(5000));

        _logger.LogInformation("Timed Hosted Service running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckPrice();
            try
            {
                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch(OperationCanceledException) { }
        }
    }
    public async Task CheckPrice()
    {

        using (IServiceScope scope = serviceScopeFactory.CreateScope())
        {
            WarframeMarketContext warframeMarketContext =
                scope.ServiceProvider.GetRequiredService<WarframeMarketContext>();


            var alertsByItem = warframeMarketContext.Alerts.Include(a => a.Item)
                                                  .ThenInclude(i => i.ItemMonitoring)
                                                  .GroupBy(a => a.ItemId)
                                                  .ToDictionary(k => k.Key, y => y
                                                    .Select(a => new {a.TgId, a.Cost, a.Item.ItemMonitoring, a.Item.UrlName})
                                                    .ToList());
            
            foreach (var alerts in alertsByItem)
            {

                Order[] allOrders = (await this._warframeMarketAPI.GetOrdersAsync(alerts.Value[0].UrlName)).payload.orders;

                var priceSell = allOrders
                            .Where(order => order.OrderType == "sell" && order.User.Status == "ingame")
                            .MinBy(order => order.Platinum)?.Platinum;

                if (priceSell == null)
                {
                    continue;
                }

                                                         
                foreach (var alert in alerts.Value)
                {
                    if (alert.Cost >= priceSell && alert.Cost < alert.ItemMonitoring.SellCost) { 
                    
                        string message = $"Цена на {alert.UrlName} стала {priceSell}! \n https://warframe.market/ru/items/{alert.UrlName}";

                        await _botClient.SendTextMessageAsync(alert.TgId, message, parseMode: ParseMode.Html);                    
                    }

                }

                alerts.Value[0].ItemMonitoring.SellCost = (int)priceSell;

            }

            await warframeMarketContext.SaveChangesAsync();
        }
        _logger.LogInformation("Чекаю лучшие предложения~");
    }

}

