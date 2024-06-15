using Warframe_market_API;

string s = "creeping_bullseye";
var wmAPI = new WarframeMarketAPI();
Console.WriteLine(await wmAPI.GetOrdersAsync(s));