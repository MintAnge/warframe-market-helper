namespace MintAnge.WarframeMarketAPI.Models;

public class Order
{
    public string OrderType { get; set; }
    public int Quantity { get; set; }
    public int Platinum { get; set; }
    public User User { get; set; }
    public string Platform { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool Visible { get; set; }
    public string Id { get; set; }
    public int ModRank { get; set; }
    public string Region { get; set; }
}
