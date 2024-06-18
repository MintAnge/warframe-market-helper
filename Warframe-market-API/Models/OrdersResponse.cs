namespace MintAnge.WarframeMarketApi.Models;

public class OrdersResponse
{
    public Payload payload { get; set; }

    public class Payload
    {
        public Order[] orders { get; set; }
    }
}


