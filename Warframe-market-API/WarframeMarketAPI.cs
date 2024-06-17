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
        public async Task<OrdersResponse> GetOrdersAsync(string s)
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, $"items/{s}/orders");
            HttpResponseMessage n = await sharedClient.SendAsync(m);
            Stream rm = await n.Content.ReadAsStreamAsync();
            //JsonDocument str = await WarframeMarketAPI.GetStream(rm);
            OrdersResponse? r = await JsonSerializer.DeserializeAsync<OrdersResponse>(rm);
            return r;
        }

        public async Task<AllItemsResponse> GetAllItemsAsync()
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, "items" );
            HttpResponseMessage n = await sharedClient.SendAsync(m);
            Stream rm = await n.Content.ReadAsStreamAsync();
            AllItemsResponse? r = await JsonSerializer.DeserializeAsync<AllItemsResponse>(rm);
            return r;
        }

        public async Task<ItemResponse> GetItemAsync(string s)
        {
            HttpRequestMessage m = new HttpRequestMessage(HttpMethod.Get, $"items/{s}");
            HttpResponseMessage n = await sharedClient.SendAsync(m);
            Stream rm = await n.Content.ReadAsStreamAsync();
            ItemResponse? r = await JsonSerializer.DeserializeAsync<ItemResponse>(rm);
            return r;
        }

        public static async Task<JsonDocument> GetStream(Stream json)
        {
            var v = await JsonDocument.ParseAsync(json);
            return v;
        }
    }
}
