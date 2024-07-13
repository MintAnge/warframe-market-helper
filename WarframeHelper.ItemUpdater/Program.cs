using Microsoft.Extensions.DependencyInjection;
using MintAnge.WarframeMarketAPI.Models;
using System.Threading;
using Microsoft.Extensions.Logging;
using MintAnge.WarframeMarketAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Item = MintAnge.WarframeHelper.Data.Models.Item;
using MintAnge.WarframeHelper.Data;

namespace MintAnge.WarframeHelper.ItemUpdater;
internal class UpdateItems
{
    private static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        PostgresConfiguration cO = builder.Configuration.GetRequiredSection("PostgresConfig").Get<PostgresConfiguration>();
        builder.Build();

        var contextOptions = new DbContextOptionsBuilder<WarframeMarketContext>()
            .UseNpgsql(cO.GetConnectionString())
            .Options;

        WarframeMarketContext warframeMarketContext = new(contextOptions);
        WarframeMarketAPI.WarframeMarketAPI warframeMarketApi = new();

        ItemShort[] items = (await warframeMarketApi.GetAllItemsAsync()).payload.items;
        Dictionary<string, string> UrlNames = warframeMarketContext.Items.Select(t => t.UrlName).ToDictionary(t => t);

        for (int i = 0; i < items.Length; i++)
        {
            if (!UrlNames.ContainsKey(items[i].UrlName))
            {
                Item item = new()
                {
                    UrlName = items[i].UrlName,
                    Description = "",
                    ItemMonitoring = new()
                    {
                        BuyCost = 0,
                        SellCost = 0
                    }

                };
                await warframeMarketContext.Items.AddAsync(item);
            }
        }

        await warframeMarketContext.SaveChangesAsync();
    }
}