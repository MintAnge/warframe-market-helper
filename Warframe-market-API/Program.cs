using System.Text.Json;
using Warframe_market_API;
using System.Linq;

string s = "creeping_bullseye";
var wmAPI = new WarframeMarketAPI();

Order[] I = (await wmAPI.GetOrdersAsync(s)).payload.orders;
IEnumerable<Order> query = I.OrderBy(order => order.platinum).Take(5);
var result = query.Select(obj =>  obj.platinum).ToList();
Console.WriteLine(JsonSerializer.Serialize((result)));


//Console.WriteLine(JsonSerializer.Serialize( (await wmAPI.GetOrdersAsync(s)).payload.orders));


//Console.WriteLine(JsonSerializer.Serialize( (await wmAPI.GetItemAsync(s)).payload.item));
//Console.WriteLine(JsonSerializer.Serialize( (await wmAPI.GetAllItemsAsync()).payload.items));
//Console.WriteLine((await wmAPI.GetOrdersAsync(s)).RootElement.GetProperty("payload").GetProperty("orders")[0].GetProperty("platinum").GetInt32());