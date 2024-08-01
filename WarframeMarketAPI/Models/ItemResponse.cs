namespace MintAnge.WarframeMarketAPI.Models;

public class ItemResponse
{ 
    public Payload payload { get; set; }

    public class Payload
    {
        public Item item { get; set; }
    }   

}