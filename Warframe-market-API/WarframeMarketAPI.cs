using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_market_API
{
    internal class WarframeMarketAPI
    {
        public static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://api.warframe.market/v1/"),
        };

        public async Task<string> GetOrdersAsync(string s)
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, $"items/{s}/orders");
            string n = await sharedClient.SendAsync(m).Result.Content.ReadAsStringAsync();
            return n;
        }

        public async Task<string> GetAllItemsAsync()
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, "items" );
            string n = await sharedClient.SendAsync(m).Result.Content.ReadAsStringAsync();
            return n;
        }

        public async Task<string> GetItemAsync(string s)
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, $"items/{s}");
            string n = await sharedClient.SendAsync(m).Result.Content.ReadAsStringAsync();
            return n;
        }
    }
}
