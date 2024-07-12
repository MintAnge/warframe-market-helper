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

        WarframeMarketContext warframeMarketContext = new WarframeMarketContext(contextOptions);

        WarframeMarketAPI warframeMarketApi = new WarframeMarketAPI();

        ItemShort[] items = (await warframeMarketApi.GetAllItemsAsync()).payload.items;

        await warframeMarketContext.ItemMonitorings.ExecuteDeleteAsync();
        await warframeMarketContext.Items.ExecuteDeleteAsync();

        for (int i =0; i<items.Length; i++)
        {
            Warframe_Helper.Data.Models.Item item = new Warframe_Helper.Data.Models.Item{
                    UrlName = items[i].UrlName,
                    Description =""
            };
            await warframeMarketContext.Items.AddAsync(item);           
        }

        await warframeMarketContext.SaveChangesAsync();
    }
}