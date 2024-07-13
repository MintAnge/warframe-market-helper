namespace MintAnge.WarframeMarketAPI.Models;

public class AllItemsResponse
{
    public Payload payload { get; set; }

    public class Payload
    {
        public ItemShort[] items { get; set; }
    }
}


