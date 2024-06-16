using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Warframe_market_API
{
    internal class WarframeMarketAPI
    {
        public static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://api.warframe.market/v1/"),
        };
        public async Task<JsonDocument> GetOrdersAsync(string s)
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, $"items/{s}/orders");
            HttpResponseMessage n = await sharedClient.SendAsync(m);
            Stream rm = await n.Content.ReadAsStreamAsync();
            var str = await WarframeMarketAPI.GetStream(rm);
            return str;
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

        public static async Task<JsonDocument> GetStream(Stream json)
        {
            var v = await JsonDocument.ParseAsync(json);
            return v;
        }
    }
}
