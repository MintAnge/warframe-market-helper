using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MintAnge.WarframeMarketAPI.Models;
 
namespace MintAnge.WarframeMarketAPI;

public class WarframeMarketAPI
{
    public  HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://api.warframe.market/v1/"),
    };
    public async Task<OrdersResponse> GetOrdersAsync(string s)
    {
        return await ExecuteRequest<OrdersResponse>($"items/{s}/orders");
    }

    public async Task<AllItemsResponse> GetAllItemsAsync()
    {
        return await ExecuteRequest<AllItemsResponse>("items");
    }

    public async Task<ItemResponse> GetItemAsync(string s)
    {
        return await ExecuteRequest<ItemResponse>($"items/{s}");
    }

    public async Task<T> ExecuteRequest<T>(string s)
    {
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, s);
        HttpResponseMessage responseMessage = await sharedClient.SendAsync(requestMessage);
        Stream stream = await responseMessage.Content.ReadAsStreamAsync();
        JsonSerializerOptions? snake = new JsonSerializerOptions();
        snake.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        return await JsonSerializer.DeserializeAsync<T>(stream, snake);
    }
}
