using Microsoft.Extensions.DependencyInjection;
using MintAnge.WarframeMarketApi.Models;
using System.Threading;
using Warframe_Helper.Data.Models;
using Warframe_Helper.Data;
using Microsoft.Extensions.Logging;
using MintAnge.WarframeMarketApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Item = Warframe_Helper.Data.Models.Item;


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
        WarframeMarketAPI warframeMarketApi = new();

        ItemShort[] items = (await warframeMarketApi.GetAllItemsAsync()).payload.items;
        Dictionary<string, string> UrlNames = warframeMarketContext.Items.Select(t => t.UrlName).ToDictionary(t => t);
        List<int> ItemsId = warframeMarketContext.Items.Select(t => t.Id).ToList();
        Dictionary<int, int> IdOrders = warframeMarketContext.ItemMonitorings.Select(t => t.ItemId).ToDictionary(t => t);

        for (int i =0; i<items.Length; i++)
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

        for (int i = 0; i < ItemsId.Count; i++)
        {
            if (!IdOrders.ContainsKey(ItemsId[i]))
            {
                ItemMonitoring itemMonitoring = new()
                { 
                        ItemId = ItemsId[i],
                        BuyCost = 0,
                        SellCost = 0
                };
                await warframeMarketContext.ItemMonitorings.AddAsync(itemMonitoring);
            }
        }

        await warframeMarketContext.SaveChangesAsync();
    }
}